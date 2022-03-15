using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent { get; private set; }

    Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        if (unit.unitState != EUnitState.Idle)
        {
            agent.SetDestination(agent.destination);
        }
    }
}
