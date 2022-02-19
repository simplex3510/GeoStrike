using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum EPlayer
{
    ally = 6,
    enemy = 7
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
    float lastAttackTime;

    protected virtual void Awake()
    {
        if (photonView.IsMine)
        {
            gameObject.layer = (int)EPlayer.ally;
            opponentLayerMask = 1 << (int)EPlayer.enemy;
        }
        else
        {
            gameObject.layer = (int)EPlayer.enemy;
            opponentLayerMask = 1 << (int)EPlayer.ally;
        }
    }

    protected virtual void Update()
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
        if(enemyCollider2D != null)
        {
            if(lastAttackTime + attackSpeed <= Time.time)
            {
                lastAttackTime = Time.time;
                OnAttack(enemyCollider2D);
            }
        }
    }

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = startHealth;
    }

    [PunRPC]
    protected abstract void OnAttack(Collider2D enemy);

    public virtual void OnDamaged(float _damage)
    {
        currentHealth -= _damage - defense;

        if (currentHealth <= 0 && isDead == false)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        isDead = true;
        // ���İ� ���̰� �ݶ��̴� ���ְ�
    }
}
