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

public abstract class Unit : MonoBehaviourPun, IDamageable, IActivatable, IBuffable, IDebuffable
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

        isPlayer1 = (photonView.ViewID / 1000) == 1 ? true : false; ;
        
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

    private void Start()
    {
        StartCoroutine(scan());
    }

    IEnumerator scan()
    {
        while(true)
        {
            Vector3 prePosition = transform.position;
            yield return new WaitForSeconds(0.5f);
            Vector3 postPosition = transform.position;

            if (prePosition == postPosition && unitState == EUnitState.Move)
            {
                SetStartAStar(null);
            }
        }
    }

    protected virtual void Update()
    {
        if (this.gameObject.name =="aaa")
        {
            Debug.Log("fsdfarsefsdf");
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
                //각 UnitClass 마다 Attck 구현
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
       // if(photonView.IsMine)
       // {
            myPool.Enqueue(this);
       // }
    }

    void Move() // 앞으로 전진
    {
        // 코루틴으로 이전 위치값과 현재 위치값을 비교한 후 MOVE 상태라면 PathFinding하기

        if (!isRotate)
        {
            StartCoroutine(RotateAnimation());
        }

        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if(enemyCollider2D == null)
        {
            SetStartAStar(null);
        }
        else
        {
            SetStartAStar(enemyCollider2D);
            unitState = EUnitState.Approach;
        }

        #region A* Move
        try
        {
            Vector2Int nextPos = new Vector2Int(aStar.finalNodeList[moveIndex].x, aStar.finalNodeList[moveIndex].y);

            if (isPlayer1)
            {
                if (aStar.finalNodeList[moveIndex].x <= transform.position.x && aStar.finalNodeList[moveIndex].y <= transform.position.y)
                {
                    if (aStar.finalNodeList.Count - 1 == moveIndex)
                    {
                        return;
                    }
                    moveIndex++;
                }
            }
            else
            {
                if (transform.position.x <= aStar.finalNodeList[moveIndex].x && transform.position.y <= aStar.finalNodeList[moveIndex].y)
                {
                    if (aStar.finalNodeList.Count - 1 == moveIndex)
                    {
                        return;
                    }
                    moveIndex++;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
           
        }
        catch (System.Exception)
        {

            throw;
        }
        #endregion

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
            #region A* Move
            Vector2Int nextPos = new Vector2Int(aStar.finalNodeList[moveIndex].x, aStar.finalNodeList[moveIndex].y);

            if (isPlayer1)
            {
                if (aStar.finalNodeList[moveIndex].x <= transform.position.x &&
                    aStar.finalNodeList[moveIndex].y <= transform.position.y)
                {
                    if (aStar.finalNodeList.Count - 1 == moveIndex)
                    {
                        return;
                    }
                    moveIndex++;
                }
            }
            else
            {
                if (transform.position.x <= aStar.finalNodeList[moveIndex].x &&
                    transform.position.y <= aStar.finalNodeList[moveIndex].y)
                {
                    if (aStar.finalNodeList.Count - 1 == moveIndex)
                    {
                        return;
                    }
                    moveIndex++;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
            #endregion

            if (!isRotate)
            {
                StartCoroutine(RotateAnimation(enemyCollider2D));
            }

            enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);

            // 적 콜라이더가 공격 범위 내에 들어왔다면 Attack FSM으로 전환
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

    // 1. 소환되어 배틀필드로 이동했을 때 - null
    // 2. 다른 상태에서 Move 상태로 전이되었을 때 - null
    // 3. 다른 상태에서 Approach 상태로 전이되었을 때 - not null
    public void SetStartAStar(Collider2D enemy)
    {
        aStar.startPos.x = (int)transform.position.x;
        aStar.startPos.y = (int)transform.position.y;

        if (enemy == null)
        {
            aStar.targetPos = aStar.endPos;
        }
        else
        {
            if(isPlayer1)
            {
                aStar.targetPos.x = Mathf.CeilToInt(enemy.transform.position.x);
                aStar.targetPos.y = Mathf.CeilToInt(enemy.transform.position.y);
            }
            else
            {
                aStar.targetPos.x = Mathf.FloorToInt(enemy.transform.position.x);
                aStar.targetPos.y = Mathf.FloorToInt(enemy.transform.position.y);
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
            SetStartAStar(null);
        }
    }
}