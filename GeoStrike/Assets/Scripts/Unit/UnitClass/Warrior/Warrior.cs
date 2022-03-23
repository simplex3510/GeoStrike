using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Warrior : Unit
{
    public Animator animator;
    public GameObject sword;

    [PunRPC]
    public void OnEnforceHealth()
    {
        deltaStatus.health += 3f;
        if (photonView.IsMine)
        {
            photonView.RPC("OnEnforceHealth", RpcTarget.Others);
        }
    }

    public void OnEnforceDamage()
    {
        deltaStatus.damage += 0.3f;
        if(photonView.IsMine)
        {
            photonView.RPC("OnEnforceDamage", RpcTarget.Others);
        }
    }

    public void OnEnforceDefense()
    {
        deltaStatus.defense += 0.2f;
        if(photonView.IsMine)
        {
            photonView.RPC("OnEnforceDefense", RpcTarget.Others);
        }
    }
    public void OnEnforceAttackSpeed()
    {
        deltaStatus.attackSpeed -= 0.2f;    // 조정 필요
        animator.speed = deltaStatus.attackSpeed;
        if (photonView.IsMine)
        {
            photonView.RPC("OnEnforceAttackSpeed", RpcTarget.Others);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        animator.speed = initStatus.attackSpeed;
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

        if (enemyCollider != null)
        {
            enemyCollider.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }
        else
        {
            unitMove.SetMove();
            unitState = EUnitState.Move;
            return;
        }
    }
}