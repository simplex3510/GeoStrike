using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum EPlayer
{
    Ally = 6,
    Enemy = 7
}

public enum EUnitState
{
    Idle,
    Move,
    Approach,
    Attack,
    Die
}

public enum EUnitIndex
{
    Warrior = 0,
    Defender,
    Shooter,
    Splasher,
    Buffer,
    Debuffer
}

public struct RowAndColumn
{
    public int row;
    public int column;
}

public interface IDamageable
{
    public void OnDamaged(float _damage);
}

public interface IActivatable
{
    public void SetUnitActive(bool _bool);
}

interface IBuffable
{
    public void OnBuff(float _buff);
}

interface IDebuffable
{
    public void OnDebuff(float _debuff);
}

public abstract class Unit : MonoBehaviourPun, IDamageable, IActivatable, IBuffable, IDebuffable, IPunObservable
{
    public UnitData initStatus;
    public UnitData deltaStatus;
    public Queue<Unit> myPool;

    // 배치상태의 위치 저장
    public UnitTile unitTile;
    public UnitCreator unitCreator;

    // 주석 추가 필요
    public RowAndColumn rowAndColumn
    {
        get
        {
            RowAndColumn.column = column;
            RowAndColumn.row = row;
            return RowAndColumn;
        }
    }
    private RowAndColumn RowAndColumn;
    public int row;
    public int column;

    // 유닛의 FSM의 상태
    public EUnitState unitState { get; protected set; }

    #region Status
    [HideInInspector]
    public EUnitIndex unitIndex;
    public string unitName { get; protected set; }
    public float startHealth { get; protected set; }
    public float currentHealth { get; protected set; }
    public float damage { get; protected set; }
    public float defense { get; protected set; }
    public float attackRange { get; protected set; }
    public float detectRange { get; protected set; }
    public float attackSpeed { get; protected set; }
    public float moveSpeed { get; protected set; }
    public bool isDead { get; protected set; }
    #endregion

    protected LayerMask opponentLayerMask;  // 공격할 대상
    protected Collider2D enemyCollider2D;
    protected Rigidbody2D myRigid2D;
    protected AStar aStar;
    protected double lastAttackTime;
    protected bool isPlayer1;

    int moveIndex = 1;
    bool isRotate;

    protected virtual void Awake()
    {
        myRigid2D = GetComponent<Rigidbody2D>();
        aStar = GetComponent<AStar>();

        isPlayer1 = (photonView.ViewID / 1000) == 1 ? true : false;

        if (photonView.IsMine)
        {
            gameObject.layer = (int)EPlayer.Ally;
            opponentLayerMask = 1 << (int)EPlayer.Enemy;
        }
        else
        {
            gameObject.layer = (int)EPlayer.Enemy;
            opponentLayerMask = 1 << (int)EPlayer.Ally;
        }

        #region Status Init
        deltaStatus.unitIndex = initStatus.unitIndex;
        deltaStatus.unitName = initStatus.unitIndex.ToString();
        deltaStatus.health = initStatus.health;
        deltaStatus.damage = initStatus.damage;
        deltaStatus.defense = initStatus.defense;
        deltaStatus.attackRange = initStatus.attackRange;
        deltaStatus.detectRange = initStatus.detectRange;
        deltaStatus.attackSpeed = initStatus.attackSpeed;
        deltaStatus.moveSpeed = initStatus.moveSpeed;
        #endregion
    }

    protected virtual void Update()
    {
        if(GameMgr.blueNexus == false || GameMgr.redNexus == false)
        {
            unitState = EUnitState.Idle;
        }

        switch (unitState)
        {
            case EUnitState.Idle:
                break;
            case EUnitState.Move:
                Move();
                break;
            case EUnitState.Approach:
                Approach();
                break;
            case EUnitState.Attack:
                //각 Unit Class 마다 Attck 구현
                break;
            case EUnitState.Die:
                Die();
                break;
        }
    }

    protected virtual void OnEnable()
    {
        isDead = false;

        #region deltaStatus Init
        unitIndex = deltaStatus.unitIndex;
        unitName = deltaStatus.unitName;
        startHealth = deltaStatus.health;
        currentHealth = startHealth;
        damage = deltaStatus.damage;
        defense = deltaStatus.defense;
        attackRange = deltaStatus.attackRange;
        detectRange = deltaStatus.detectRange;
        attackSpeed = deltaStatus.attackSpeed;
        moveSpeed = deltaStatus.moveSpeed;
        #endregion

        unitState = EUnitState.Idle;
    }

    // Return to your pool
    protected virtual void OnDisable()
    {
        if (photonView.IsMine)
        {
            myPool.Enqueue(this);
        }
    }

    void Move() // 앞으로 전진
    {
        if (!isRotate)
        {
            StartCoroutine(RotateAnimation());
        }

        if(transform.position.x == aStar.targetPos.x &&
           transform.position.y == aStar.targetPos.y)
        {
            SetStartAStar(null);
        }

        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if (enemyCollider2D == null)
        {
            if (aStar.finalNodeList.Count == 0)
            {
                SetStartAStar(null);
                return;
            }

            #region A* Move
            Vector2Int nextPos = new Vector2Int(aStar.finalNodeList[moveIndex].x, aStar.finalNodeList[moveIndex].y);

            if (isPlayer1)
            {
                if (aStar.finalNodeList[moveIndex].x <= transform.position.x &&
                    aStar.finalNodeList[moveIndex].y <= transform.position.y)
                {
                    if (moveIndex <= aStar.finalNodeList.Count - 1)
                    {
                        moveIndex++;
                    }
                }
            }
            else
            {
                if (transform.position.x <= aStar.finalNodeList[moveIndex].x &&
                    transform.position.y <= aStar.finalNodeList[moveIndex].y)
                {
                    if (moveIndex <= aStar.finalNodeList.Count - 1)
                    {
                        moveIndex++;
                    }
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
            #endregion
        }
        else
        {
            SetStartAStar(enemyCollider2D);
            unitState = EUnitState.Approach;
        }
    }

    void Approach() // 적에게 접근
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if (enemyCollider2D == null)
        {
            SetStartAStar(null);
            unitState = EUnitState.Move;
            return;
        }
        else
        {
            if (!isRotate)
            {
                StartCoroutine(RotateAnimation(enemyCollider2D));
            }

            if (aStar.finalNodeList.Count == 0)
            {
                SetStartAStar(enemyCollider2D);
                return;
            }

            #region A* Move
            Vector2Int nextPos = new Vector2Int(aStar.finalNodeList[moveIndex].x, aStar.finalNodeList[moveIndex].y);

            if (isPlayer1)
            {
                if (aStar.finalNodeList[moveIndex].x <= transform.position.x &&
                    aStar.finalNodeList[moveIndex].y <= transform.position.y)
                {
                    if (moveIndex <= aStar.finalNodeList.Count - 1)
                    {
                        moveIndex++;
                    }
                }
            }
            else
            {
                if (transform.position.x <= aStar.finalNodeList[moveIndex].x &&
                    transform.position.y <= aStar.finalNodeList[moveIndex].y)
                {
                    if (moveIndex <= aStar.finalNodeList.Count - 1)
                    {
                        moveIndex++;
                    }
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
            #endregion

            enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
            if (enemyCollider2D != null)
            {
                unitState = EUnitState.Attack;
                return;
            }
        }
    }

    public abstract void Attack();   // 적에게 공격

    void Die()    // 유닛 사망
    {
        isDead = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(DieAnimation(gameObject));
    }

    [PunRPC]
    public void OnDamaged(float _damage)
    {
        _damage -= defense;
        currentHealth -= 0 < _damage ? _damage : 0;

        if (currentHealth <= 0 && isDead == false)
        {
            unitState = EUnitState.Die;
        }
    }

    [PunRPC]
    public void SetUnitActive(bool isTrue)
    {
        this.gameObject.SetActive(isTrue);
        if (photonView.IsMine)
        {
            photonView.RPC("SetUnitActive", RpcTarget.Others, isTrue);
        }
    }

    [PunRPC]
    public void OnBuff(float _buffDamage)
    {

    }

    [PunRPC]
    public void OnDebuff(float _debuffDamage)
    {

    }

    protected IEnumerator DieAnimation(GameObject _gameObject)
    {
        unitState = EUnitState.Idle;

        var spriteRenderer = _gameObject.GetComponent<SpriteRenderer>();
        var color = spriteRenderer.color;
        while (0 <= color.a)
        {
            color.a -= 1f * Time.deltaTime;
            spriteRenderer.color = color;

            yield return null;
        }

        _gameObject.SetActive(false);
        spriteRenderer.color = Color.white;
    }

    // 앞을 바라보는 애니메이션
    IEnumerator RotateAnimation()
    {
        isRotate = true;

        if (isPlayer1)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 1f);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, 180f), 1f);
        }

        isRotate = false;
        yield return null;
    }

    // enemy를 바라보는 애니메이션
    IEnumerator RotateAnimation(Collider2D enemy)
    {
        isRotate = true;

        Vector3 direct = enemy.transform.position - transform.position;     // 방향을 구함
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // 두 객체 간의 각을 구함
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // 최종적으로 회전해야 하는 회전값

        while (!Mathf.Approximately(transform.rotation.z, target.z))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 1.5f);

            yield return null;
        }

        isRotate = false;
    }

    private void OnApplicationQuit()
    {
        #region Return Status Init
        deltaStatus.health = initStatus.health;
        deltaStatus.damage = initStatus.damage;
        deltaStatus.defense = initStatus.defense;
        deltaStatus.attackRange = initStatus.attackRange;
        deltaStatus.detectRange = initStatus.detectRange;
        deltaStatus.attackSpeed = initStatus.attackSpeed;
        deltaStatus.moveSpeed = initStatus.moveSpeed;
        #endregion
    }

    public void SetStartAStar(Collider2D target)
    {
        if (target == null)
        {
            if (isPlayer1)
            {
                aStar.bottomLeft.x = Mathf.CeilToInt(transform.position.x - detectRange);
                aStar.bottomLeft.y = Mathf.CeilToInt(transform.position.y - detectRange);

                aStar.topRight.x = Mathf.CeilToInt(transform.position.x + detectRange);
                aStar.topRight.y = Mathf.CeilToInt(transform.position.y + detectRange);

                aStar.startPos.x = Mathf.CeilToInt(transform.position.x);
                aStar.startPos.y = Mathf.CeilToInt(transform.position.y);

                //targetPos 설정
                aStar.targetPos.x = Mathf.FloorToInt(transform.position.x + detectRange);
                aStar.targetPos.y = Mathf.FloorToInt(transform.position.y);
            }
            else
            {
                aStar.bottomLeft.x = Mathf.FloorToInt(transform.position.x - detectRange);
                aStar.bottomLeft.y = Mathf.FloorToInt(transform.position.y - detectRange);

                aStar.topRight.x = Mathf.FloorToInt(transform.position.x + detectRange);
                aStar.topRight.y = Mathf.FloorToInt(transform.position.y + detectRange);

                aStar.startPos.x = Mathf.FloorToInt(transform.position.x);
                aStar.startPos.y = Mathf.FloorToInt(transform.position.y);

                //targetPos 설정
                aStar.targetPos.x = Mathf.FloorToInt(transform.position.x - detectRange);
                aStar.targetPos.y = Mathf.FloorToInt(transform.position.y);
            }

            
        }
        else
        {
            if (isPlayer1)
            {
                aStar.bottomLeft.x = Mathf.CeilToInt(transform.position.x - detectRange);
                aStar.bottomLeft.y = Mathf.CeilToInt(transform.position.y - detectRange);

                aStar.topRight.x = Mathf.CeilToInt(transform.position.x + detectRange);
                aStar.topRight.y = Mathf.CeilToInt(transform.position.y + detectRange);

                aStar.startPos.x = Mathf.CeilToInt(transform.position.x);
                aStar.startPos.y = Mathf.CeilToInt(transform.position.y);

                aStar.targetPos.x = Mathf.CeilToInt(target.transform.position.x);
                aStar.targetPos.y = Mathf.CeilToInt(target.transform.position.y);
            }
            else
            {
                aStar.bottomLeft.x = Mathf.FloorToInt(transform.position.x - detectRange);
                aStar.bottomLeft.y = Mathf.FloorToInt(transform.position.y - detectRange);

                aStar.topRight.x = Mathf.FloorToInt(transform.position.x + detectRange);
                aStar.topRight.y = Mathf.FloorToInt(transform.position.y + detectRange);

                aStar.startPos.x = Mathf.FloorToInt(transform.position.x);
                aStar.startPos.y = Mathf.FloorToInt(transform.position.y);

                aStar.targetPos.x = Mathf.FloorToInt(target.transform.position.x);
                aStar.targetPos.y = Mathf.FloorToInt(target.transform.position.y);
            }
        }

        moveIndex = 1;

        aStar.PathFinding();
    }

    public void SetFreezeNone()
    {
        myRigid2D.constraints = RigidbodyConstraints2D.None;
    }

    public void SetFreezeAll()
    {
        myRigid2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Idle에서 Move가 되는 조건
    public IEnumerator IdleToMoveCondition()
    {
        while (GameMgr.instance.GetState() == EGameState.FSM_SpawnCount)
        {
            yield return null;
        }

        unitState = EUnitState.Move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            print("아군과 충돌하여 새로운 경로 탐색");
            if(enemyCollider2D == null)
            {
                SetStartAStar(null);
            }
            else
            {
                SetStartAStar(enemyCollider2D);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine)
        {
            stream.SendNext(unitState);
        }
        else
        {
            unitState = (EUnitState)stream.ReceiveNext();
        }
    }
}