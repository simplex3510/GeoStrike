using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slpasher : Unit
{
    //public Animator animator;
    public Transform grenadeSpawnPos;
    public BulletPool bulletPool;
    public Grenade grenade;

    protected override void Awake()
    {
        base.Awake();
        bulletPool = GetComponent<BulletPool>();
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

            grenade = bulletPool.GetBullet();                                    // ����ü ����
            grenade.transform.position = grenadeSpawnPos.position;  // ����ü�� ��ġ�� ����
            grenade.transform.rotation = this.transform.rotation;                // ����ü�� ȸ���� ����
            grenade.damage = this.damage;                                        // ����ü ����� ����
            grenade.targetCollider = enemyColliders[0];                          // ����ü�� ��ǥ�� ����
            grenade.SetBulletActive(true);                                       // ����ü Ȱ��ȭ

        }
        else if (enemyColliders.Length == 0)
        {
            unitMove.agent.isStopped = false;
            unitState = EUnitState.Move;
            return;
        }
    }
}
