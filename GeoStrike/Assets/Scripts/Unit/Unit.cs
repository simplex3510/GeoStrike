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
    Debuffer,
    Geo
}

[SerializeField]
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
    public void OnBuff(int _buffType, float _buff);
    public void OffBuff(int _buffType, float _buff);
}

interface IDebuffable
{
    public void Debuff(int _debuffType, float _debuff, float _applyTime);
    public IEnumerator OnDebuff(int _debuffType, float _debuff, float _applyTime);
}
[SelectionBase] // �θ� ����
public abstract class Unit : MonoBehaviourPun, IDamageable, IActivatable, IBuffable, IDebuffable, IPunObservable
{
    // �������ͽ�
    public InitUnitData initStatus;
    public DeltaUnitData deltaStatus;

    // 오브젝트 풀 연결
    public Queue<Unit> myPool;

    // ��ġ������ ��ġ ���� Components
    [HideInInspector] public UnitTile unitTile;
    [HideInInspector] public UnitCreator unitCreator;

    // Unit의 Spawn 위치 저장 
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

    // ������ �ൿ�� ���� ���
    public LayerMask opponentLayerMask { get; protected set; }

    // ������ FSM�� ����
    /*[HideInInspector]*/
    public EUnitState unitState;

    // ������ ��ü(��������Ʈ)
    public GameObject body;

    // Unit UI Interfaceâ
    private Detector detector;

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

    protected Collider enemyCollider;   // ���� ���
    protected UnitMove unitMove;
    protected Rigidbody rigidBody;
    protected double lastAttackTime;
    protected bool isPlayer1;

    public bool IsStun { get { return isStun; } private set { isStun = value; } }
    bool isStun;
    bool hasDebuff;

    public AudioSource theAudio;

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        unitMove = GetComponent<UnitMove>();
        detector = GameObject.FindObjectOfType<Detector>();

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

        #region deltaStatus Init
        unitIndex = deltaStatus.unitIndex;
        unitName = deltaStatus.unitName;
        startHealth = deltaStatus.Health;
        currentHealth = startHealth;
        damage = deltaStatus.Damage;
        defense = deltaStatus.Defense;
        attackRange = deltaStatus.AttackRange;
        detectRange = deltaStatus.DetectRange;
        attackSpeed = deltaStatus.AttackSpeed;
        moveSpeed = deltaStatus.MoveSpeed;
        #endregion
    }

    Vector3 startPos;
    protected virtual void OnEnable()
    {
        #region deltaStatus Init
        unitIndex = deltaStatus.unitIndex;
        unitName = deltaStatus.unitName;
        startHealth = deltaStatus.Health;
        currentHealth = startHealth;
        damage = deltaStatus.Damage;
        defense = deltaStatus.Defense;
        attackRange = deltaStatus.AttackRange;
        detectRange = deltaStatus.DetectRange;
        attackSpeed = deltaStatus.AttackSpeed;
        moveSpeed = deltaStatus.MoveSpeed;
        #endregion

        startPos = this.transform.position;
        unitState = EUnitState.Idle;
    }

    // Return to your pool
    protected virtual void OnDisable()
    {
        if (photonView.IsMine)
        {
            myPool.Enqueue(this);
        }
        else
        {
            transform.position = startPos;
        }

        if(PhotonNetwork.IsMasterClient)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 270, 0);
        }
    }

    protected virtual void Update()
    {
        switch (unitState)
        {
            case EUnitState.Idle:
                break;
            case EUnitState.Move:
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                //�� Unit Class ���� Attck ����
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

    public virtual void Attack() { }

    protected virtual void Die()    // ���� ���
    {
        unitMove.enabled = false;
        unitMove.agent.enabled = false;
        StartCoroutine(DieAnimation(body));
    }

    public virtual void Stun(float _stunTime)
    {
        StartCoroutine(OnStun(_stunTime));

        if (photonView.IsMine)
        {
            photonView.RPC("OnStun", RpcTarget.Others, _stunTime);
        }
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

    public IEnumerator OnStun(float _stunTime)
    {
        unitState = EUnitState.Idle;

        float currentTime = 0;
        while(currentTime <= _stunTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        unitState = EUnitState.Attack;
    }

    public void OnEnforceDamage()
    {
        deltaStatus.Damage += 300f;
        if (photonView.IsMine)
        {
            photonView.RPC("OnEnforceDamage", RpcTarget.Others);
        }
        print("OnEnforceDamage");
    }

    public void OnEnforceDefense()
    {
        deltaStatus.Defense += 200f;
        if (photonView.IsMine)
        {
            photonView.RPC("OnEnforceDefense", RpcTarget.Others);
        }
        print("OnEnforceDefense");
    }

    public void OnEnforceHealth()
    {
        deltaStatus.Health += 300f;
        if (photonView.IsMine)
        {
            photonView.RPC("OnEnforceHealth", RpcTarget.Others);
        }
        print("OnEnforceHealth");
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
    [PunRPC]
    public void OnBuff(int _buffType, float _buff)
    {
        if(GetComponent<Buffer>() != null)
        {
            return;
        }

        switch((EBuffandDebuff)_buffType)
        {
            case EBuffandDebuff.Damage:
                damage += _buff;
                if (photonView.IsMine) { photonView.RPC("OnBuff", RpcTarget.Others, _buffType, _buff); }
                break;
        }
    }

    [PunRPC]
    public void OffBuff(int _buffType, float _buff)
    {
        switch ((EBuffandDebuff)_buffType)
        {
            case EBuffandDebuff.Damage:
                damage -= _buff;
                if (photonView.IsMine) { photonView.RPC("OffBuff", RpcTarget.Others, _buffType, _buff); }
                break;
        }
    }

    [PunRPC]
    public void Debuff(int _debuffType, float _debuff, float _applyTime)
    {
        StartCoroutine(OnDebuff(_debuffType,  _debuff, _applyTime));
    }

    public IEnumerator OnDebuff(int _debuffType, float _debuff, float _applyTime)
    {
        float time = 0;
        switch ((EBuffandDebuff)_debuffType)
        {
            case EBuffandDebuff.Damage:
                hasDebuff = false;
                damage -= _debuff;
                if (photonView.IsMine) { photonView.RPC("Debuff", RpcTarget.Others, _debuffType, _debuff, _applyTime); }
                while (time <= _applyTime) { time += Time.deltaTime; yield return null; }
                break;
        }
    }
    #endregion

    [PunRPC]
    protected IEnumerator DieAnimation(GameObject _gameObject)
    {
        unitState = EUnitState.Idle;
        gameObject.GetComponent<Collider>().enabled = false;

        // Ŭ���� ������ ������ �������̽� â �ʱ�ȭ
        if (detector.clickedObject == this.gameObject)
        {
            detector.InitInterface();
        }

        enemyCollider = null;

        //var spriteRenderer = _gameObject.GetComponent<SpriteRenderer>();
        //var color = spriteRenderer.color;
        //while (0 <= color.a)
        //{
        //    color.a -= 1.5f * Time.deltaTime;
        //    spriteRenderer.color = color;

        //    yield return null;
        //}

        if(_gameObject.name == "Body")
        {
            SetUnitActive(false);
        }

        //spriteRenderer.color = Color.white;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.SetActive(false);                                            // Pool로 되돌아가는 시점
        transform.position = ObjectPoolMgr.instance.transform.position;         // 유닛 위치 초기화
        yield return null;
    }

    protected virtual void OnApplicationQuit()
    {
        #region Return Status Init
        deltaStatus.unitIndex = initStatus.unitIndex;
        deltaStatus.unitName = initStatus.unitName;
        deltaStatus.Health = initStatus.Health;
        deltaStatus.Damage = initStatus.Damage;
        deltaStatus.Defense = initStatus.Defense;
        deltaStatus.AttackRange = initStatus.AttackRange;
        deltaStatus.DetectRange = initStatus.DetectRange;
        deltaStatus.AttackSpeed = initStatus.AttackSpeed;
        deltaStatus.MoveSpeed = initStatus.MoveSpeed;
        #endregion
    }

    // Idle에서 Move로 전환 조건
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