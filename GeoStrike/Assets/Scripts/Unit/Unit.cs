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

interface IDamageable
{
    public void OnDamaged(float _damage);
}

interface IActivatable
{
    public void SetUnitActive(bool _bool);
}

public abstract class Unit : MonoBehaviourPun, IDamageable, IActivatable
{
    public UnitData initStatus;
    public UnitData deltaStatus;
    public Queue<Unit> myPool;

    // ��ġ������ ��ġ ����
    public UnitTile unitTile;
    public UnitCreator unitCreator;

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
    protected EUnitState unitState;         // ������ FSM�� ����
    protected Collider2D enemyCollider2D;
    protected Rigidbody2D myRigid2D;
    protected double lastAttackTime;
    protected bool isPlayer1;

    bool isRotate;

    protected virtual void Awake()
    {
        myRigid2D = GetComponent<Rigidbody2D>();

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
        if(photonView.IsMine)
        {
            myPool.Enqueue(this);
        }
    }

    protected virtual void Update()
    {
        // �ڽ��� ������Ʈ�� �ƴ϶�� �������� ����
        if(!photonView.IsMine)
        {
            return;
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
                //Animation���� Attck() ȣ��
                break;
            case EUnitState.Die:
                Die();
                break;
        }
    }

    void Move() // ������ ����
    {
        if (isPlayer1)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        if (!isRotate)
        {
            StartCoroutine(RotateAnimation());
        }

        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if (enemyCollider2D != null)
        {
            unitState = EUnitState.Approach;
            return;
        }
    }

    void Approach() // ������ ����
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if (enemyCollider2D != null)
        {
            transform.position += (enemyCollider2D.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;

            if (!isRotate)
            {
                StartCoroutine(RotateAnimation(enemyCollider2D));
            }

            if ((enemyCollider2D.transform.position - transform.position).magnitude <= attackRange)
            {
                unitState = EUnitState.Attack;
                return;
            }
            else if (enemyCollider2D == null)
            {
                unitState = EUnitState.Move;
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

    // ���� �ٶ󺸴� �ִϸ��̼�
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

    // enemy�� �ٶ󺸴� �ִϸ��̼�
    IEnumerator RotateAnimation(Collider2D enemy)
    {
        isRotate = true;

        Vector3 direct = enemy.transform.position - transform.position;     // ������ ����
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // �� ��ü ���� ���� ����
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // ���������� ȸ���ؾ� �ϴ� ȸ����

        while (!Mathf.Approximately(transform.rotation.z, target.z))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 1.5f);

            yield return null;
        }

        isRotate = false;
    }

    private void OnApplicationQuit()
    {
        deltaStatus.health = initStatus.health;
        deltaStatus.damage = initStatus.damage;
        deltaStatus.defense = initStatus.defense;
        deltaStatus.attackRange = initStatus.attackRange;
        deltaStatus.detectRange = initStatus.detectRange;
        deltaStatus.attackSpeed = initStatus.attackSpeed;
        deltaStatus.moveSpeed = initStatus.moveSpeed;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, attackRange);
        //Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    public EUnitState GetUnitState()
    {
        return unitState;
    }

    public void SetFreezeNone()
    {
        myRigid2D.constraints = RigidbodyConstraints2D.None;
    }

    public void SetFreezeAll()
    {
        myRigid2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
