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

    public Transform test;

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

        targetColliders = Physics.OverlapSphere(transform.position, detectRange, opponentLayerMask);
        if (1 == targetColliders.Length)
        {
            agent.SetDestination(allyNexus.position);
        }
        else
        {
            if (targetColliders[0].gameObject == this.gameObject)
            {
                target = targetColliders[1].gameObject;
            }
            else
            {
                target = targetColliders[0].gameObject;
            }

            if (target.gameObject.activeSelf == false)
            {
                float temp;
                float distance;
                int targetIndex = -1;

                distance = 10;
                for (int i = 0; i < targetColliders.Length; i++)
                {
                    if (targetColliders[i].gameObject == this.gameObject)
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

                target = targetColliders[targetIndex].gameObject;
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

        targetColliders = Physics.OverlapSphere(transform.position, detectRange, opponentLayerMask);  // 범위 내 아군 탐색
        Transform short_enemy = null;

        if (targetColliders.Length > 0)
        {
            float short_distance = Mathf.Infinity;

            foreach (Collider s_trg in targetColliders)
            {
                float playertoenemy = Vector3.SqrMagnitude(this.transform.position - s_trg.transform.position);

                if (short_distance > playertoenemy)
                {
                    short_distance = playertoenemy;
                    short_enemy = s_trg.transform;
                }
            }

            test = short_enemy;
            agent.SetDestination(target.transform.position * 1.05f);
        }

        //if (1 < targetColliders.Length && target.activeSelf != false)                                 // 범위 내 아군이 있다면
        //{
        //    agent.SetDestination(target.transform.position * 1.05f);                                          // 아군에게 접근
        //}
        //else                                                                                          // 범위 내 아군이 없다면
        //{
        //    unitState = EUnitState.Move;                                                              // 아군 넥서스로 이동
        //}
    }

    new void Die()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DieAnimation(body));
    }
    #endregion
}