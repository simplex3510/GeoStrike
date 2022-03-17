using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buffer : Unit
{
    public Transform allyNexus;
    public Animator animator;
    public GameObject body;
    public GameObject buff;

    Collider[] targetColliders;
    float buffRange;
    float buffDamage = 2f;    // Buff Status Delta

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

    protected new void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if (GameMgr.blueNexus == false || GameMgr.redNexus == false)
        {
            unitState = EUnitState.Idle;
        }

        switch (unitState)
        {
            case EUnitState.Idle:
                break;
            case EUnitState.Move:
                Move();
                break;
            case EUnitState.Approach:
                Approach();
                break;
            case EUnitState.Attack:
                break;
            case EUnitState.Die:
                Die();
                break;
        }
    }

    #region FSM
    void Move()
    {
        targetColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);
        if (1 == targetColliders.Length)
        {
            unitMove.agent.destination = allyNexus.position;                                    // 목적지를 아군 넥서스로 설정
        }
        else    // 자신을 제외한 콜라이더
        {
            unitMove.agent.destination = targetColliders[1].transform.position;                  // 목적지를 아군으로 설정
            unitState = EUnitState.Approach;
        }
    }

    void Approach() // 아군에게 접근
    {
        targetColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);  // 범위 내 아군 탐색
        if (1 < targetColliders.Length)                                                                                    // 범위 내 아군이 있다면
        {
            unitMove.agent.destination = targetColliders[1].transform.position;
        }
        else                                                                                                              // 범위 내 아군이 없다면
        {
            unitMove.agent.destination = allyNexus.position;                                                              // 아군 넥서스 쪽으로 이동
            unitState = EUnitState.Move;                                                                                  // 계속 이동
        }
    }

    void Die()    // 유닛 사망
    {
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DieAnimation(body));
    }
    #endregion


}