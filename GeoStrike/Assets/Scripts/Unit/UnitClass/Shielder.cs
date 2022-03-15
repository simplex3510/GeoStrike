using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Shielder : Unit
{
    public Animator animator;
    public GameObject shild;

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
                StartCoroutine(DieAnimation(shild));
                break;
        }
    }

    public override void Attack()   // ������ ����
    {
        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask);
        if (enemyColliders.Length == 0)
        {
            unitState = EUnitState.Move;
            return;
        }
        else
        {
            enemyColliders[0].GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }
    }
}