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

public enum EBuffandDebuff
{
    Damage,
    Defence,
    Haste
    // 추가
}

public struct RowAndColumn
{
    public int row;
    public int column;
}

interface IDamageable
{
    public void OnDamaged(float _damage);
}

interface IActivatable
{
    public void SetUnitActive(bool _bool);
}

interface IBuffable
{
    public void OnBuff(EBuffandDebuff _buffType, float _buff);
    public void OffBuff(EBuffandDebuff _DebuffType, float _buff);
}

interface IDebuffable
{
    public void OnDebuff(EBuffandDebuff _DebuffType, float _debuff);
    public void OffDebuff(EBuffandDebuff _DebuffType, float _debuff);
}

public abstract class Unit : MonoBehaviourPun, IDamageable, IActivatable, IBuffable, IDebuffable, IPunObservable
{
    public UnitData initStatus;
    public UnitData deltaStatus;
    public Queue<Unit> myPool;

    // 배치상태의 위치 저장 Components
    [HideInInspector] public UnitTile unitTile;
    [HideInInspector] public UnitCreator unitCreator;

    // Unit의 Spawn 위치 기억 
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
    [HideInInspector] public int row;
    [HideInInspector] public int column;

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
    protected UnitMove unitMove;
    protected Collider[] enemyColliders;    // 현재 범위에 들어온 모든 enemy
    protected Rigidbody myRigid;
    protected double lastAttackTime;
    protected bool isPlayer1;

    bool hasBuff;
    bool hasDebuff;
    bool isRotate;

    protected virtual void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
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
                //각 Unit Class 마다 Attck 구현
                myRigid.velocity = Vector3.zero;
                unitMove.agent.velocity = Vector3.zero;
                unitMove.agent.isStopped = true;
                break;
            case EUnitState.Die:
                Die();
                break;
        }
    }

    #region FSM
    void Move() // 앞으로 전진
    {
        //if (!isRotate)
        //{
        //    StartCoroutine(RotateAnimation());
        //}

        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);
        if (enemyColliders.Length == 0)
        {
            unitMove.agent.destination = unitMove.target.position;
        }
        else 
        {
            unitMove.agent.destination = enemyColliders[0].transform.position;
            unitState = EUnitState.Approach;
        }
    }

    void Approach() // 적에게 접근
    {
        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);
        if (enemyColliders.Length == 0)
        {
            unitState = EUnitState.Move;
            return;
        }
        else
        {
            //if (!isRotate)
            //{
            //    StartCoroutine(RotateAnimation(enemyColliders[0]));
            //}

            enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask);
            if (enemyColliders.Length != 0)
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
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DieAnimation(gameObject));
    }
    #endregion 

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
    public void OnBuff(EBuffandDebuff buffType, float _buff)
    {
        switch(buffType)
        {
            case EBuffandDebuff.Damage:
                if (!hasBuff) { hasBuff = true; return; }
                damage += _buff;
                if (photonView.IsMine) { photonView.RPC("OnBuff", RpcTarget.Others, _buff); }
                break;
        }
    }

    public void OffBuff(EBuffandDebuff buffType, float _buff)
    {
        switch (buffType)
        {
            case EBuffandDebuff.Damage:
                hasBuff = false;
                damage -= _buff;
                if (photonView.IsMine) { photonView.RPC("OnBuff", RpcTarget.Others, _buff); }
                break;
        }
    }

    public void OnDebuff(EBuffandDebuff buffType, float _debuff)
    {
        switch(buffType)
        {
            case EBuffandDebuff.Damage:
                break;
        }
    }

    public void OffDebuff(EBuffandDebuff buffType, float _debuff)
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
        gameObject.GetComponent<Collider>().enabled = true;
        spriteRenderer.color = Color.white;
    }

    // 앞을 바라보는 애니메이션
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

    // enemy를 바라보는 애니메이션
    IEnumerator RotateAnimation(Collider enemy)
    {
        isRotate = true;

        Vector3 direct = enemy.transform.position - transform.position;     // 방향을 구함
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // 두 객체 간의 각을 구함
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.up);   // 최종적으로 회전해야 하는 회전값

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
        myRigid.constraints = RigidbodyConstraints.None;
    }

    public void SetFreezeAll()
    {
        myRigid.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Idle에서 Move가 되는 조건
    public IEnumerator IdleToMoveCondition()
    {
        while (GameMgr.instance.GetState() == EGameState.SpawnCount)
        {
            yield return null;
        }

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