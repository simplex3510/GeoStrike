using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Shielder : Unit
{
    public Animator animator;
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
                animator.SetBool("isMove", true);
                animator.SetBool("isAttack", false);
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                animator.SetBool("isMove", false);
                if (weapon.activeSelf)
                {
                    animator.SetBool("isAttack", true);
                }
                break;
            case EUnitState.Die:
                StartCoroutine(DieAnimation(weapon));
                break;
        }
    }

    // 방패가 부숴졌을때
    // 상태 변수들 초기화 시키기 (공격 애니메이션 등등..)
    private void DistroyShield()
    {

    }
}