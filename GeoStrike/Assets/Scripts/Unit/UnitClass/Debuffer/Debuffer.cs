using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Debuffer : Unit
{
    Collider[] enemyColliders;
    EBuffandDebuff currentDebuff;
    float debuffDeltaStatus = 2f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        switch (unitState)
        {
            case EUnitState.Idle:
                break;
            case EUnitState.Move:
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                Attack();
                break;
            case EUnitState.Die:
                Die();
                break;
        }
    }

    public override void Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask);

        if (enemyColliders.Length != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                enemyColliders[i].GetComponent<Unit>().OnDamaged(damage);
                enemyColliders[i].GetComponent<Unit>().OnDebuff((int)currentDebuff, debuffDeltaStatus);
            }
        }
        else if (enemyColliders.Length == 0)
        {
            unitMove.SetMove();
            unitState = EUnitState.Move;
            return;
        }
    }

    protected override void Die()
    {
        if(enemyColliders.Length != 0)
        {
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                enemyColliders[0].GetComponent<Unit>().OffDebuff((int)currentDebuff, debuffDeltaStatus);
            }
        }
    }
}
