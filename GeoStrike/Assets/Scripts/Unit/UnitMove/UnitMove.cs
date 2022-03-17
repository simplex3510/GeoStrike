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
        if (enemyColliders.Length == 0)     // Ž�� ���� ���� ���� ���ٸ�
        {
            agent.destination = enemyNexus.position;
        }
        else                                // Ž�� ���� ���� ���� �ִٸ�
        {
            agent.destination = enemyColliders[0].transform.position;
            unit.unitState = EUnitState.Approach;
        }
    }

    void Approach() // ������ ����
    {
        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
        if (enemyColliders.Length == 0)         // Ž�� ���� ���� ���� ���ٸ�
        {
            unit.unitState = EUnitState.Move;
            return;
        }
        else
        {
            enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.attackRange, unit.opponentLayerMask);
            if (enemyColliders.Length != 0)     // ���� ���� ���� ���� �ִٸ�
            {
                rigidBody.velocity = Vector3.zero;
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                // agent.updatePosition = false;    // �� �κп��� �������� �������� �ʰ� Y������ �����

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
