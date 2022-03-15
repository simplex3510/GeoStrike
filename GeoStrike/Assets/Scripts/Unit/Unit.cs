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
    public void OffBuff(float _buff);
}

interface IDebuffable
{
    public void OnDebuff(float _debuff);
    public void OffDebuff(float _debuff);
}

public abstract class Unit : MonoBehaviourPun, IDamageable, IActivatable, IBuffable, IDebuffable, IPunObservable
{
    public UnitData initStatus;
    public UnitData deltaStatus;
    public Queue<Unit> myPool;

    // ��ġ������ ��ġ ����
    public UnitTile unitTile;
    public UnitCreator unitCreator;

    // �ּ� �߰� �ʿ�
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

    // ������ FSM�� ����
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

    protected LayerMask opponentLayerMask;  // ������ ���
    protected UnitMove unitMove;
    protected Collider2D enemyCollider2D;
    protected Rigidbody2D myRigid2D;
    protected double lastAttackTime;
    protected bool isPlayer1;

    int moveIndex = 1;
    bool isRotate;

    protected virtual void Awake()
    {
        myRigid2D = GetComponent<Rigidbody2D>();
        unitMove = GetComponent<UnitMove>();

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

        unitState = EUnitState.Move;
    }

    // Return to your pool
    protected virtual void OnDisable()
    {
        if (photonView.IsMine)
        {
            myPool.Enqueue(this);
        }
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
                //�� Unit Class ���� Attck ����
                break;
            case EUnitState.Die:
                Die();
                break;
        }
    }

    void Move() // ������ ����
    {
        if (!isRotate)
        {
            StartCoroutine(RotateAnimation());
        }

        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if (enemyCollider2D == null)
        {
            unitMove.agent.destination = unitMove.target.position;
        }
        else 
        {
            unitMove.agent.destination = enemyCollider2D.transform.position;
            unitState = EUnitState.Approach;
        }
        
    }

    void Approach() // ������ ����
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if (enemyCollider2D == null)
        {
            unitState = EUnitState.Move;
            return;
        }
        else
        {
            if (!isRotate)
            {
                StartCoroutine(RotateAnimation(enemyCollider2D));
            }

            enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
            if (enemyCollider2D != null)
            {
                unitState = EUnitState.Attack;
                return;
            }
        }
    }

    public abstract void Attack();   // ������ ����

    void Die()    // ���� ���
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

    #region buff & debuff
    public void OnBuff(float _buff)
    {
        damage += _buff;
        if(photonView.IsMine)
        {
            photonView.RPC("OnBuff", RpcTarget.Others, _buff);
        }

    }

    public void OffBuff(float _buff)
    {
        damage -= _buff;
        if (photonView.IsMine)
        {
            photonView.RPC("OffBuff", RpcTarget.Others, _buff);
        }
    }

    public void OnDebuff(float _debuff)
    {

    }

    public void OffDebuff(float _debuff)
    {

    }
    #endregion

    #region animation
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
        gameObject.GetComponent<Collider2D>().enabled = true;
        spriteRenderer.color = Color.white;
    }

    // ���� �ٶ󺸴� �ִϸ��̼�
    IEnumerator RotateAnimation()
    {
        isRotate = true;

        if (isPlayer1)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(90f, 0f, 0f), 1f);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(90f, 180f, 0f), 1f);
        }

        isRotate = false;
        yield return null;
    }

    // enemy�� �ٶ󺸴� �ִϸ��̼�
    IEnumerator RotateAnimation(Collider2D enemy)
    {
        isRotate = true;

        Vector3 direct = enemy.transform.position - transform.position;     // ������ ����
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // �� ��ü ���� ���� ����
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.up);   // ���������� ȸ���ؾ� �ϴ� ȸ����

        while (!Mathf.Approximately(transform.rotation.z, target.z))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 1.5f);

            yield return null;
        }

        isRotate = false;
    }
    #endregion

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

    public void SetFreezeNone()
    {
        myRigid2D.constraints = RigidbodyConstraints2D.None;
    }

    public void SetFreezeAll()
    {
        myRigid2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Idle���� Move�� �Ǵ� ����
    public IEnumerator IdleToMoveCondition()
    {
        while (GameMgr.instance.GetState() == EGameState.SpawnCount)
        {
            yield return null;
        }

        Debug.Log("Set Move State");
        unitState = EUnitState.Move;
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