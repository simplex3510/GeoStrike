using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Debuffer : Unit
{
    Collider[] enemyColliders;
    float debuffDamage = 2f;

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
        if (!photonView.IsMine)
        {
            return;
        }

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
                break;
        }
    }

    public override void Attack()
    {
        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask);

        if (enemyColliders.Length != 0 && lastAttackTime + attackSpeed <= PhotonNetwork.Time)
        {
            lastAttackTime = PhotonNetwork.Time;

            for (int i = 0; i < 4; i++)
            {
                enemyColliders[i].GetComponent<Unit>().OnDamaged(damage);
                enemyColliders[i].GetComponent<Unit>().OnDebuff(EBuffandDebuff.Damage, debuffDamage);
            }
        }
        else if (enemyColliders.Length == 0)
        {
            unitState = EUnitState.Move;
            return;
        }
    }
}
