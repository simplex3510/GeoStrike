using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

enum EPlayer
{
    Ally = 6,
    Enemy = 7
}

enum EUnitState
{
    Move,
    Approach,
    Attack,
    Die
}

public abstract class Unit : MonoBehaviourPun, IDamageable
{
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

    Collider2D enemyCollider2D;
    EUnitState unitState;
    float lastAttackTime;
    bool isPlayer1;

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
        StartCoroutine(FSM());
    }

    protected virtual IEnumerator FSM()
    {
        while (true)
        {
            switch (unitState)
            {
                case EUnitState.Move:
                    Move();
                    break;
                case EUnitState.Approach:
                    Approach();
                    break;
                case EUnitState.Attack:
                    Attack();
                    break;
                case EUnitState.Die:
                    Die();
                    break;
            }
            yield return null;
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

        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if (enemyCollider2D != null)
        {
            unitState = EUnitState.Approach;
            return;
        }
    }

    void Approach() // 적에게 접근
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        transform.position = Vector3.MoveTowards(transform.position, enemyCollider2D.transform.position, moveSpeed * Time.deltaTime);

        if (enemyCollider2D != null && (transform.position - enemyCollider2D.transform.position).magnitude <= attackRange)
        {
            unitState = EUnitState.Attack;
            return;
        }
    }

    void Attack()   // 적에게 공격
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
        if (enemyCollider2D != null && lastAttackTime + attackSpeed <= PhotonNetwork.Time)
        {
            lastAttackTime = (float)PhotonNetwork.Time;
            enemyCollider2D.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }
        else if (enemyCollider2D == null)
        {
            unitState = EUnitState.Move;
            return;
        }

    }

    protected virtual void Die()
    {
        StartCoroutine(DieAnimation());
    }

    [PunRPC]
    public virtual void OnDamaged(float _damage)
    {
        currentHealth -= _damage - defense;

        if (currentHealth <= 0 && isDead == false)
        {
            unitState = EUnitState.Die;
        }
    }

    IEnumerator DieAnimation()
    {
        isDead = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        var color = spriteRenderer.color;
        while (0 <= color.a)
        {
            color.a -= 1f * Time.deltaTime;
            spriteRenderer.color = color;

            yield return null;
        }

        gameObject.SetActive(false);
        spriteRenderer.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
