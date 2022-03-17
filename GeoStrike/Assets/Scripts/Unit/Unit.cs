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
    // 스테이터스
    public UnitData initStatus;
    public UnitData deltaStatus;

    // 오브젝트 풀
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

    // 유닛이 행동을 취할 대상
    public LayerMask opponentLayerMask { get; protected set; }

    // 유닛의 FSM의 상태
    /*[HideInInspector]*/public EUnitState unitState;

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
    #endregion

    protected Collider enemyCollider;   // 공격 대상
    protected UnitMove unitMove;
    protected Rigidbody rigidBody;
    protected double lastAttackTime;
    protected bool isPlayer1;

    bool hasBuff;
    bool hasDebuff;

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
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

    protected virtual void Update()
    {
        switch (unitState)
        {
            case EUnitState.Idle:
                return;
            case EUnitState.Move:
                return;
            case EUnitState.Approach:
                return;
            case EUnitState.Attack:
                //각 Unit Class 마다 Attck 구현
                break;
            case EUnitState.Die:
                Die();
                break;
        }

        if (GameMgr.blueNexus == false || GameMgr.redNexus == false)
        {
            unitState = EUnitState.Idle;
        }
    }

    public virtual void Attack() { }   // 적에게 공격

    void Die()    // 유닛 사망
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }

    [PunRPC]
    public void OnDamaged(float _damage)
    {
        _damage -= defense;
        currentHealth -= 0 < _damage ? _damage : 0;

        if (currentHealth <= 0)
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

    protected IEnumerator DieAnimation(GameObject _gameObject)
    {
        unitState = EUnitState.Idle;

        var spriteRenderer = _gameObject.GetComponent<SpriteRenderer>();
        var color = spriteRenderer.color;
        while (0 <= color.a)
        {
            color.a -= 1.5f * Time.deltaTime;
            spriteRenderer.color = color;

            yield return null;
        }

        gameObject.GetComponent<Collider>().enabled = true;
        spriteRenderer.color = Color.white;

        if(_gameObject.name == "Body")
        {
            SetUnitActive(false);
        }
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

    public void SetFreezeNone()
    {
        rigidBody.constraints = RigidbodyConstraints.None;
    }

    public void SetFreezeAll()
    {
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
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