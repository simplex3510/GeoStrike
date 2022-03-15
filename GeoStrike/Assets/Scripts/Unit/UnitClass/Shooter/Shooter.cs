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
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
        if (enemyCollider2D != null && lastAttackTime + attackSpeed <= PhotonNetwork.Time)
        {
            lastAttackTime = PhotonNetwork.Time;

            bullet = bulletPool.GetBullet();                                    // 투사체 생성
            bullet.transform.position = bulletSpawnPos[bulletPosIdx].position;  // 투사체의 위치값 설정
            bullet.transform.rotation = this.transform.rotation;                // 투사체의 회전값 설정
            bullet.damage = this.damage;                                        // 투사체 대미지 설정
            bullet.targetCollider2D = enemyCollider2D;                          // 투사체의 목표를 설정
            bullet.SetBulletActive(true);                                       // 투사체 활성화

            bulletPosIdx = bulletPosIdx > 0 ? 0 : 1;
        }
        else if (enemyCollider2D == null)
        {
            unitState = EUnitState.Move;
            return;
        }
    }
}
