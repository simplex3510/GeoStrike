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
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);
        if (enemyCollider2D == null)
        {
            SetStartAStar(null);
            unitState = EUnitState.Move;
            return;
        }
        else
        {
            enemyCollider2D.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }
    }
}