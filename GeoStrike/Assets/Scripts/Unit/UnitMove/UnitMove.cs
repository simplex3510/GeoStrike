using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    public NavMeshAgent agent { get; private set; }
    public Transform enemyNexus;

    Unit unit;
    Collider[] enemyColliders;
    Rigidbody rigidBody;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        agent.destination = transform.position;

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    private void Update()
    {
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
        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
        if (enemyColliders.Length == 0)     // 탐지 범위 내에 적이 없다면
        {
            agent.destination = enemyNexus.position;
        }
        else                                // 탐지 범위 내에 적이 있다면
        {
            agent.destination = enemyColliders[0].transform.position;
            unit.unitState = EUnitState.Approach;
        }
    }

    void Approach() // 적에게 접근
    {
        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
        if (enemyColliders.Length == 0)         // 탐지 범위 내에 적이 없다면
        {
            unit.unitState = EUnitState.Move;
            return;
        }
        else
        {
            enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.attackRange, unit.opponentLayerMask);
            if (enemyColliders.Length != 0)     // 공격 범위 내에 적이 있다면
            {
                rigidBody.velocity = Vector3.zero;
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                // agent.updatePosition = false;    // 이 부분에서 포지션이 고정되지 않고 Y축으로 상승함

                unit.unitState = EUnitState.Attack;
                return;
            }
        }
    }

    void Die()
    {
        agent.destination = transform.position;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.updatePosition = false;
        agent.updateRotation = false;
    }
    #endregion 
}
