using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class UnitMove : MonoBehaviourPun
{
    public NavMeshAgent agent { get; private set; }
    public Transform enemyNexus;

    Unit unit;
    Collider[] enemyColliders;
    Vector3 destination;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();
    }

    private void OnEnable()
    {
        SetMove();
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        switch (unit.unitState)
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
        agent.SetDestination(destination);  // 실질적 이동

        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
        if (enemyColliders.Length == 0)     // 탐지 범위 내에 적이 없다면
        {
            destination = enemyNexus.position;
        }
        else                                // 탐지 범위 내에 적이 있다면
        {
            destination = enemyColliders[0].transform.position;
            unit.unitState = EUnitState.Approach;
            return;
        }
    }

    void Approach() // 적에게 접근
    {
        agent.SetDestination(destination);      // 실질적 이동

        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
        if (enemyColliders.Length == 0)         // 탐지 범위 내에 적이 없어졌다면
        {
            destination = enemyNexus.position;
            unit.unitState = EUnitState.Move;
            return;
        }
        else                                    // 탐지 범위 내에 적이 있고
        {
            enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.attackRange, unit.opponentLayerMask);
            if (enemyColliders.Length != 0)     // 공격 범위 내에 적이 있다면
            {
                SetStop();
                unit.unitState = EUnitState.Attack;
                return;
            }
        }
    }

    void Die()
    {
        SetStop();
    }
    #endregion 

    public void SetMove()
    {
        agent.isStopped = false;
        agent.updatePosition = true;
    }

    public void SetStop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.updatePosition = false;
    }
}
