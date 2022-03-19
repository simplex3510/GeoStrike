using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooter : Unit
{
    //public Animator animator;
    public Transform[] bulletSpawnPos;
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
                StartCoroutine(DieAnimation(body));
                break;
        }
    }

    public override void Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        enemyCollider = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask).Length != 0 ?
                        Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask)[0] :
                        null;

        if (enemyCollider != null && lastAttackTime + attackSpeed <= PhotonNetwork.Time)
        {
            lastAttackTime = PhotonNetwork.Time;

            bullet = bulletPool.GetBullet();                                    // ����ü ����
            bullet.transform.position = bulletSpawnPos[bulletPosIdx].position;  // ����ü�� ��ġ�� ����
            bullet.damage = this.damage;                                        // ����ü ����� ����
            bullet.targetCollider = enemyCollider;                              // ����ü�� ��ǥ�� ����
            bullet.SetBulletActive(true);                                       // ����ü Ȱ��ȭ

            bulletPosIdx = 0 != bulletPosIdx ? 0 : 1;
        }
        else if (enemyCollider == null)
        {
            unitMove.SetMove();
            unitState = EUnitState.Move;
            return;
        }
    }
}
