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
        if (!photonView.IsMine)
        {
            return;
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

        if (GameMgr.blueNexus == false || GameMgr.redNexus == false)
        {
            unitState = EUnitState.Idle;
        }
    }

    #region FSM
    void Move()
    {
        try
        {
            if (!photonView.IsMine || !GetComponent<NavMeshAgent>().enabled)
            {
                return;
            }

            targetColliders = Physics.OverlapSphere(transform.position, detectRange, opponentLayerMask);
            if (targetColliders.Length <= 1)
            {
                if (!agent.SetDestination(allyNexus.position))
                {
                    agent.enabled = false;
                    agent.enabled = true;
                }
            }
            else
            {
                if (target == null)
                {
                    float temp;
                    float distance = detectRange;
                    int targetIndex = -1;

                    // ?????????? ???? ?????? ????
                    for (int i = 0; i < targetColliders.Length; i++)
                    {
                        if (targetColliders[i].gameObject == this.gameObject ||     // ???? ?????????? ??????
                            targetColliders[i].GetComponent<Buffer>() != null)      // ???????? ??????
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
                        if(!agent.SetDestination(allyNexus.position))
                        {
                            agent.enabled = false;
                            agent.enabled = true;
                        }
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
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    void Approach() // ???????? ????
    {
        try
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (target.activeSelf != false)                                 // ???? ?? ?????? ??????
            {
                agent.SetDestination(target.transform.position * 0.85f);    // ???????? ????
            }
            else                                                            // ???? ?? ?????? ??????
            {
                target = null;
                unitState = EUnitState.Move;                                // ???? ???????? ????
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
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