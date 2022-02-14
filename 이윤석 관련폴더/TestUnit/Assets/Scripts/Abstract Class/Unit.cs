using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IDamageable
{
    public bool isDead { get; protected set; }
    public float startHealth { get; protected set; }
    public float currentHealth { get; protected set; }
    public float damage { get; protected set; }
    public float defense { get; protected set; }
    public float range { get; protected set; }
    public float speed { get; protected set; }


    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = startHealth;
    }

    public virtual void OnDamaged(float _damage)
    {
        currentHealth -= _damage;

        if(currentHealth <= 0 && isDead == false)
        {
            Die();  
        }
    }

    public virtual void Die()
    {
        isDead = true;
        // 알파값 줄이고 콜라이더 없애고
    }
}
