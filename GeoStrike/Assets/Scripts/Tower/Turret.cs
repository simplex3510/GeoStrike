using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret : Tower
{
    public BulletPool bulletPool;
    public Bullet bullet;
    protected double lastAttackTime;

    protected override void Awake()
    {
        base.Awake();
        bulletPool = GetComponent<BulletPool>();
    }

    private void Update()
    {
        switch (towerState)
        {
            case ETowerState.Idle:
                break;
            case ETowerState.Attack:
                Attack();
                break;
        }
    }

    public void Attack()
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

            bullet = bulletPool.GetBullet();                                    // 투사체 생성
            bullet.transform.position = this.transform.position;                // 투사체의 위치값 설정
            bullet.damage = this.damage;                                        // 투사체 대미지 설정
            bullet.targetCollider = enemyCollider;                              // 투사체의 목표를 설정
            bullet.SetBulletActive(true);                                       // 투사체 활성화
        }
        else if (enemyCollider == null)
        {
            towerState = ETowerState.Idle;
            return;
        }
    }
}
