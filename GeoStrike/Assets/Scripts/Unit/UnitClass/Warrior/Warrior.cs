using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Warrior : Unit
{
    public Animator animator;
    public GameObject sword;

    protected override void Awake()
    {
        base.Awake();
        animator.speed = initStatus.AttackSpeed;
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
                break;
        }
    }

    public override void Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        enemyCollider = Physics.OverlapSphere(transform.position, attackRange, opponentLayerMask).Length != 0 ?
                        Physics.OverlapSphere(transform.position, attackRange, opponentLayerMask)[0] :
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

    public void AttackSound()
    {
        Debug.Log("Attack");

        theAudio.clip = clip;
        theAudio.Play();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}