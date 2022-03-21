using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Buffer : Unit
{
    public Transform allyNexus;
    public Animator animator;
    public GameObject buff;

    NavMeshAgent agent;
    Collider[] targetColliders;
    float buffRange;
    float buffDamage = 2f;    // Delta Buff Status 

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
        agent = GetComponent<NavMeshAgent>();
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

        if (GameMgr.blueNexus == false || GameMgr.redNexus == false)
        {
            unitState = EUnitState.Idle;
        }
    }

    #region FSM
    void Move()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        targetColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);
        if (1 == targetColliders.Length)
        {
            agent.SetDestination(allyNexus.position);                                  // 목적지를 아군 넥서스로 설정
        }
        else                                                                           // 자신을 제외한 콜라이더
        {
            unitState = EUnitState.Approach;
        }
    }

    void Approach() // 아군에게 접근
    {
        if (!photonView.IsMine)
        {
            return;
        }

        targetColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);  // 범위 내 아군 탐색
        if (1 < targetColliders.Length)                                                                                    // 범위 내 아군이 있다면
        {
            agent.SetDestination(targetColliders[1].transform.position - new Vector3(0.8f, 0f, 0.8f));                         // 아군에게 접근
        }
        else                                                                                                               // 범위 내 아군이 없다면
        {
            unitState = EUnitState.Move;                                                                                   // 아군 넥서스로 이동
        }
    }

    new void Die()    // 유닛 사망
    {
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DieAnimation(body));
    }
    #endregion
}