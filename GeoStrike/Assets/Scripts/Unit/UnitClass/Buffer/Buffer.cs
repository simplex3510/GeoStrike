using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Buffer : Unit
{
    public GameObject weapon;

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

    public override void Attack()   // 적에게 공격
    {
        opponentLayerMask = 1 << (int)EPlayer.Ally;
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
        if (enemyCollider2D != null && (enemyCollider2D.name != "Buffer_Red" || enemyCollider2D.name != "Buffer_Blue"))
        {
            enemyCollider2D.GetComponent<PhotonView>().RPC("OnBuff", RpcTarget.All, damage);
        }
        else
        {
            unitState = EUnitState.Move;
            return;
        }
    }
}