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

public abstract class Unit : MonoBehaviourPun, IDamageable, IActivatable
{
    public Transform myParent;
    public Queue<Unit> myPool;

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

    // Return to your pool
    protected virtual void OnDisable()
    {
        transform.SetParent(myParent);
        myPool.Enqueue(this);
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
            case EUnitState.Approach:
                Approach();
                break;
            case EUnitState.Attack:
                //Animation?????? Attck() ??????
                break;
            case EUnitState.Die:
                Die();
                break;
        }
    }

    void Move() // ????????? ??????
    {
        if (isPlayer1)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        if(!isRotate)
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

    void Approach() // ????????? ??????
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, detectRange, opponentLayerMask);
        if (enemyCollider2D != null)
        {
            transform.position += (enemyCollider2D.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;

            if(!isRotate)
            {
                StartCoroutine(RotateAnimation(enemyCollider2D));
            }

            if ((enemyCollider2D.transform.position - transform.position).magnitude <= attackRange)
            {
                unitState = EUnitState.Attack;
                return;
            }
            else if(enemyCollider2D == null)
            {
                unitState = EUnitState.Move;
                return;
            }
        }
    }

    void Attack()   // ????????? ??????
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
        if (enemyCollider2D != null/* && lastAttackTime + attackSpeed <= PhotonNetwork.Time*/)
        {
            //lastAttackTime = (float)PhotonNetwork.Time;
            enemyCollider2D.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }
        else if (enemyCollider2D == null)
        {
            unitState = EUnitState.Move;
            StartCoroutine(RotateAnimation());
            return;
        }
    }

    protected virtual void Die()    // ?????? ??????
    {
        isDead = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        StartCoroutine(DieAnimation(gameObject));
    }

    [PunRPC]
    public void OnDamaged(float _damage)
    {
        float damage = _damage - defense;
        currentHealth = 0 < damage ? damage : 0;

        if (currentHealth <= 0 && isDead == false)
        {
            unitState = EUnitState.Die;
        }
    }

    [PunRPC]
    public void SetActive(bool isTrue)
    {
        gameObject.SetActive(isTrue);
        if(photonView.IsMine)
        {
            photonView.RPC("SetActive", RpcTarget.Others, isTrue);
        }
    }

    protected IEnumerator DieAnimation(GameObject _gameObject)
    {
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

        unitState = EUnitState.Idle;
    }

    // ?????? ???????????? ???????????????
    IEnumerator RotateAnimation()
    {
        isRotate = true;

        if(isPlayer1)
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

    // enemy??? ???????????? ???????????????
    IEnumerator RotateAnimation(Collider2D enemy)
    {
        isRotate = true;

        Vector3 direct = enemy.transform.position - transform.position;     // ????????? ??????
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // ??? ?????? ?????? ?????? ??????
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // ??????????????? ???????????? ?????? ?????????

        while (!Mathf.Approximately(transform.rotation.z, target.z))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 0.5f);

            yield return null;
        }

        isRotate = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
