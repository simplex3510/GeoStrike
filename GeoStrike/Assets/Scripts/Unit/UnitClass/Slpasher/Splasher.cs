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

    Collider[] enemyColliders;

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
        if (!photonView.IsMine)
        {
            return;
        }

        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask);

        if (enemyColliders.Length != 0 && lastAttackTime + attackSpeed <= PhotonNetwork.Time)
        {
            lastAttackTime = PhotonNetwork.Time;

            grenade = grenadePool.GetGrenade();                                 // 투사체 생성
            grenade.transform.position = grenadeSpawnPos.position;              // 투사체의 위치값 설정
            grenade.damage = this.damage;                                       // 투사체 대미지 설정
            grenade.targetCollider = enemyColliders[0];                         // 투사체의 목표를 설정
            grenade.targetPos = enemyColliders[0].transform.position;           // 투사체의 폭발 위치 지정
            grenade.parent = this.transform;                                    // 투사체 부모 설정 (폭발후 다시 부모의 하위로 돌아감)
            grenade.transform.SetParent(null);                                  // 투사체 공격 유지 : 부모가 죽어도 이미 발사된 투사체는 계속해서 나아감
            grenade.SetGrenadeActive(true);                                     // 투사체 활성화
            grenade.SetShootActive(true);                                       // 투사체 이펙트 활성화
            grenade.SetExplosionActive(false);                                  // 투사체 폭발 이펙트 비활성화
        }
        else if (enemyColliders.Length == 0)
        {
            unitMove.SetMove();
            unitState = EUnitState.Move;
            return;
        }
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
