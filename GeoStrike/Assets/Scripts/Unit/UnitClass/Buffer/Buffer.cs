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
    GameObject target;

    float buffRange;
    float buffDamage = 2f;    // Delta Buff Status

    [PunRPC]
    public void OnEnforceStartHealth()
    {
        deltaStatus.Health += 5;
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
        if(!photonView.IsMine || !GetComponent<NavMeshAgent>().enabled)
        {
            return;
        }

        targetColliders = Physics.OverlapSphere(transform.position, detectRange, opponentLayerMask);
        if (targetColliders.Length <= 1)
        {
            agent.SetDestination(allyNexus.position);
        }
        else
        {
            if (target == null)
            {
                float temp;
                float distance = detectRange;
                int targetIndex = -1;

                // 대상들과의 최소 거리를 찾음
                for (int i = 0; i < targetColliders.Length; i++)
                {
                    if (targetColliders[i].gameObject == this.gameObject ||     // 자기 자신이라면 건너뜀
                        targetColliders[i].GetComponent<Buffer>() != null)      // 버퍼라면 건너뜀
                    {
                        continue;
                    }

                    temp = Vector3.Distance(this.transform.position, targetColliders[i].transform.position);

                    if (temp < distance)
                    {
                        distance = temp;
                        targetIndex = i;
                    }
                }

                if (targetIndex == -1)
                {
                    agent.SetDestination(allyNexus.position);
                    return;
                }
                else
                {
                    target = targetColliders[targetIndex].gameObject;
                }
            }

            unitState = EUnitState.Approach;
        }
    }

    void Approach() // 아군에게 접근
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (target.activeSelf != false)                                 // 범위 내 아군이 있다면
        {
            agent.SetDestination(target.transform.position * 0.9f);    // 아군에게 접근
        }
        else                                                            // 범위 내 아군이 없다면
        {
            target = null;
            unitState = EUnitState.Move;                                // 아군 넥서스로 이동
        }
    }

    new void Die()
    {
        agent.enabled = false;
        StartCoroutine(DieAnimation(body));
    }
    #endregion

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}