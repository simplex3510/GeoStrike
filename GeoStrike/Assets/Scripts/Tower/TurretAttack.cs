using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretAttack : Unit
{
    Collider[] enemyColliders;
    float debuffDeltaStatus = 2f;
    float applyTime = 2f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        unitState = EUnitState.Move;
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

        enemyColliders = Physics.OverlapSphere(transform.position, attackRange, opponentLayerMask);

        if (enemyColliders.Length != 0 && lastAttackTime + attackSpeed <= PhotonNetwork.Time)
        {
            lastAttackTime = PhotonNetwork.Time;
            for (int i = 0; i < 4 && i < enemyColliders.Length; i++)
            {
                enemyColliders[i].GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
            }
        }
        else if (enemyColliders.Length == 0)
        {
            unitMove.SetMove();
            unitState = EUnitState.Move;
        }
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
