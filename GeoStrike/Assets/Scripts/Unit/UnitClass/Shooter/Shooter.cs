using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooter : Unit
{
    //public Animator animator;
    public Transform[] bulletSpawnPos = new Transform[2];
    public BulletPool bulletPool;
    public Bullet bullet;

    int bulletPosIdx = 0;

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
        if(!photonView.IsMine)
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
        if (enemyColliders != null && lastAttackTime + attackSpeed <= PhotonNetwork.Time)
        {
            lastAttackTime = PhotonNetwork.Time;

            bullet = bulletPool.GetBullet();                                    // ����ü ����
            bullet.transform.position = bulletSpawnPos[bulletPosIdx].position;  // ����ü�� ��ġ�� ����
            bullet.transform.rotation = this.transform.rotation;                // ����ü�� ȸ���� ����
            bullet.damage = this.damage;                                        // ����ü ����� ����
            bullet.targetCollider = enemyColliders[0];                          // ����ü�� ��ǥ�� ����
            bullet.SetBulletActive(true);                                       // ����ü Ȱ��ȭ

            bulletPosIdx = bulletPosIdx > 0 ? 0 : 1;
        }
        else if (enemyColliders == null)
        {
            unitState = EUnitState.Move;
            return;
        }
    }
}
