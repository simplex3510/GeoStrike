using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum EPlayer
{
    Player1 = 6,
    Player2 = 7
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
    protected EPlayer enemy;


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

        if(currentHealth <= 0 && isDead == false)
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
