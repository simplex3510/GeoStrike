using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buffer : Unit
{
    public Animator animator;
    public GameObject buff;

    EBuffandDebuff currentBuff;
    float buffRange;

    // Buff Status Delta
    float buffDamage = 2f;

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
        opponentLayerMask = 1 << (int)EPlayer.Ally;
        buffRange = attackRange;
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
                break;
            case EUnitState.Die:
                break;
        }
    }

    public override void Attack()   // 적에게 공격(없음)
    {
        return;
    }
}