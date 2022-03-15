using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    public Unit unit;

    // Start is called before the first frame update
    void Awake()
    {
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.destination == null)
        {
            agent.destination = target.position;
        }

        transform.rotation = Quaternion.Euler(90f, 0, 0);

        if (unit.unitState != EUnitState.Idle)
        {
            agent.SetDestination(agent.destination);
        }
    }
}
