using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bot : Unit
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        unitState = EUnitState.Idle;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();
    }

    [PunRPC]
    public new void OnDamaged(float _damage)
    {
        float damage = _damage - defense;
        currentHealth -= 0 < damage ? damage : 0;

        if (currentHealth <= 0 && isDead == false)
        {
            unitState = EUnitState.Die;
        }
    }
}