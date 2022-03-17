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

            grenade = grenadePool.GetGrenade();                                 // 투사체 생성
            grenade.transform.position = grenadeSpawnPos.position;              // 투사체의 위치값 설정
            grenade.transform.rotation = this.transform.rotation;               // 투사체의 회전값 설정
            grenade.damage = this.damage;                                       // 투사체 대미지 설정
            grenade.targetCollider = enemyColliders[0];                         // 투사체의 목표를 설정
            grenade.targetPos = enemyColliders[0].transform.position;           // 투사체의 폭발 위치 지정
            grenade.SetGrenadeActive(true);                                     // 투사체 활성화

        }
        else if (enemyColliders.Length == 0)
        {
            unitMove.agent.isStopped = false;
            unitState = EUnitState.Move;
            return;
        }
    }
}
