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
        try
        {
            agent.SetDestination(destination);  // ������ �̵�

            enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
            if (enemyColliders.Length == 0)     // Ž�� ���� ���� ���� ���ٸ�
            {
                destination = enemyNexus.position;
            }
            else                                // Ž�� ���� ���� ���� �ִٸ�
            {
                destination = enemyColliders[0].transform.position;

                unit.unitState = EUnitState.Approach;
                return;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    void Approach() // ������ ����
    {
        agent.SetDestination(destination);      // ������ �̵�

        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, unit.detectRange, unit.opponentLayerMask);
        if (enemyColliders.Length == 0)         // Ž�� ���� ���� ���� �������ٸ�
        {
            destination = enemyNexus.position;

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
            else
            {
                unit.unitState = EUnitState.Approach;
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
        if (agent.enabled == true)
        {
            agent.speed = 3f;
            //agent.isStopped = false;
            //agent.updatePosition = true;
        }
    }

    public void SetStop()
    {
        if (agent.enabled == true)
        {
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            //agent.isStopped = true;
            //agent.updatePosition = false; 
        }

    }
}
