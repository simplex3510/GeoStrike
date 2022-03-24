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
        agent.SetDestination(destination);  // ������ �̵�

        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
        if (enemyColliders.Length == 0)     // Ž�� ���� ���� ���� ���ٸ�
        {
            if(!unit.IsKnockback)
            {
                agent.SetDestination(enemyNexus.position);
            }
        }
        else                                // Ž�� ���� ���� ���� �ִٸ�
        {
            if(!unit.IsKnockback)
            {
                destination = enemyColliders[0].transform.position;
                agent.SetDestination(destination);
            }
            unit.unitState = EUnitState.Approach;
            return;
        }
    }

    void Approach() // ������ ����
    {
        if(!unit.IsKnockback)
        {
            agent.SetDestination(destination);      // ������ �̵�
        }

        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
        if (enemyColliders.Length == 0)         // Ž�� ���� ���� ���� �������ٸ�
        {
            if(!unit.IsKnockback)
            {
                agent.SetDestination(enemyNexus.position);
            }
            unit.unitState = EUnitState.Move;
            return;
        }
        else                                    // Ž�� ���� ���� ���� �ְ�
        {
            enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.attackRange, unit.opponentLayerMask);
            if (enemyColliders.Length != 0)     // ���� ���� ���� ���� �ִٸ�
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
