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

            bullet = bulletPool.GetBullet();                                    // ����ü ����
            bullet.transform.position = this.transform.position;                // ����ü�� ��ġ�� ����
            bullet.damage = this.damage;                                        // ����ü ����� ����
            bullet.targetCollider = enemyCollider;                              // ����ü�� ��ǥ�� ����
            bullet.SetBulletActive(true);                                       // ����ü Ȱ��ȭ
        }
        else if (enemyCollider == null)
        {
            towerState = ETowerState.Idle;
            return;
        }
    }
}
