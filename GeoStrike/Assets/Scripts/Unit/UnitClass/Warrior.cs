using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Warrior : Unit
{
    public Animator animator;
    public GameObject sword;

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
                animator.SetBool("isMove", true);
                animator.SetBool("isAttack", false);
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                animator.SetBool("isMove", false);
                animator.SetBool("isAttack", true);
                break;
            case EUnitState.Die:
                StartCoroutine(DieAnimation(sword));
                break;
        }
    }

    public override void Attack()   // 적에게 공격
    {
        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask);
        if (enemyColliders.Length != 0)
        {
            enemyColliders[0].GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }
        else
        {
            unitMove.agent.isStopped = false;
            unitState = EUnitState.Move;
            return;
        }
    }
}