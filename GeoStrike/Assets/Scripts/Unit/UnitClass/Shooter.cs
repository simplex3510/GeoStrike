using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooter : Unit
{
    //public Animator animator;
    public Transform[] bulletSpawnPos = new Transform[2];
    public Bullet bullet;

    public BulletPool bulletPool;

    int bulletPosIdx = 0;

    [PunRPC]
    public void OnEnforceStartHealth()
    {
        deltaStatus.health += 5;
        if (photonView.IsMine)
        {
            photonView.RPC("OnEnforceStartHealth", RpcTarget.Others);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        bulletPool = GetComponent<BulletPool>();
        bullet.bulletName = bullet.name;
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
                break;
        }
    }

    public override void Attack()
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
        if (enemyCollider2D != null && lastAttackTime + attackSpeed <= PhotonNetwork.Time)
        {
            lastAttackTime = PhotonNetwork.Time;

            bulletPosIdx = bulletPosIdx > 0 ? 0 : 1;
            bullet = bulletPool.GetBullet();    // °ø°Ý(Áø)
            bullet.gameObject.name += bulletPosIdx;
            bullet.transform.rotation = this.transform.rotation;
            bullet.damage = this.damage;
            bullet.targetCollider2D = enemyCollider2D;
            bullet.transform.position = bulletSpawnPos[bulletPosIdx].position;
            //bullet.startPosition = bulletSpawnPos[bulletPosIdx].position;
        }
        else if (enemyCollider2D == null)
        {
            unitState = EUnitState.Move;
            return;
        }
    }
}
