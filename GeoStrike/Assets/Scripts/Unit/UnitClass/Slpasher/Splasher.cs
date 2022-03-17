using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Splasher : Unit
{
    //public Animator animator;
    public Transform grenadeSpawnPos;
    public GrenadePool grenadePool;
    public Grenade grenade;

    protected override void Awake()
    {
        base.Awake();
        grenadePool = GetComponent<GrenadePool>();
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

            grenade = grenadePool.GetGrenade();                                 // ����ü ����
            grenade.transform.position = grenadeSpawnPos.position;              // ����ü�� ��ġ�� ����
            grenade.transform.rotation = this.transform.rotation;               // ����ü�� ȸ���� ����
            grenade.damage = this.damage;                                       // ����ü ����� ����
            grenade.targetCollider = enemyColliders[0];                         // ����ü�� ��ǥ�� ����
            grenade.targetPos = enemyColliders[0].transform.position;           // ����ü�� ���� ��ġ ����
            grenade.SetGrenadeActive(true);                                     // ����ü Ȱ��ȭ

        }
        else if (enemyColliders.Length == 0)
        {
            unitMove.agent.isStopped = false;
            unitState = EUnitState.Move;
            return;
        }
    }
}
