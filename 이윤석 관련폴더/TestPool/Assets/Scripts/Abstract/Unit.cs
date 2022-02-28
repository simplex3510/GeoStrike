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

public abstract class Unit : MonoBehaviourPun/*, IDamageable, IActivatable*/
{
    public Transform myParent;  // This Object must move to pool
    public Queue<Unit> myPool;  // This Object must enqueue

    public float startHealth { get; protected set; }
    public float currentHealth { get; protected set; }
    public float damage { get; protected set; }
    public float defense { get; protected set; }
    public float attackRange { get; protected set; }
    public float detectRange { get; protected set; }
    public float attackSpeed { get; protected set; }
    public float moveSpeed { get; protected set; }
    public bool isDead { get; protected set; }

    protected LayerMask opponentLayerMask;
    protected EUnitState unitState;

    Collider2D enemyCollider2D;
    // float lastAttackTime;
    bool isPlayer1;
    bool isRotate;

    protected virtual void Awake()
    {
        isPlayer1 = PhotonNetwork.IsMasterClient;

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
    }

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = startHealth;
        unitState = EUnitState.Move;
    }

    protected virtual void Update()
    {
        switch (unitState)
        {
            case EUnitState.Idle:
                break;
            case EUnitState.Move:
                Move();
                break;
        }
    }

    void Move() // 앞으로 전진
    {
        if (isPlayer1)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }
}
