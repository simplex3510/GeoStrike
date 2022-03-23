using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Defender : Unit
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

        shild.layer = this.gameObject.layer;
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
                StartCoroutine(DieAnimation(shild));
                break;
        }
    }

    public override void Attack()   // 적에게 공격
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
            Vector3 knockbackPos;

            enemyCollider.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);

            // 방향 벡터 * 넉백 오프셋

            if(enemyCollider.GetComponent<Unit>() != null)
            {
                knockbackPos = enemyCollider.transform.position.normalized * 1.5f;  // 방향 벡터 * 밀려남 오프셋
                enemyCollider.GetComponent<Unit>().StartCoroutine(enemyCollider.GetComponent<Unit>().OnKnockback(knockbackPos));
            }
        }
        else
        {
            unitMove.SetMove();
            unitState = EUnitState.Move;
            return;
        }
    }
}