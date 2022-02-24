using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(201)]
public class TranslocateField : MonoBehaviourPun
{
    // Unit state
    public List<Unit> p1UnitList = new List<Unit>();
    public List<Unit> p2UnitList = new List<Unit>();

    // Waiting unit translocate
    [SerializeField] private Transform moveToBattleFieldP1;
    [SerializeField] private Transform moveToBattleFieldP2;

    // Waiting unit parent
    public Transform spawnPosP1;
    public Transform spawnPosP2;
    private Vector3 originPosP1;
    private Vector3 originPosP2;

    private void Awake()
    {
        InitPos();
    }

    private void InitPos()
    {
        originPosP1 = spawnPosP1.transform.position;
        originPosP2 = spawnPosP2.transform.position;
    }

    public void TranslocateUnits() 
    {
        spawnPosP1.position = moveToBattleFieldP1.position;
        spawnPosP2.position = moveToBattleFieldP2.position;

        ClearList();
    }

    private void ClearList()
    {
        for (int idx = 0; idx < p1UnitList.Count; idx++)
        {
            p1UnitList[idx].transform.parent = null;
        }
        p1UnitList.Clear();
        spawnPosP1.transform.position = originPosP1;

        for (int idx = 0; idx < p2UnitList.Count; idx++)
        {
            p2UnitList[idx].transform.parent = null;
        }
        p2UnitList.Clear();
        spawnPosP2.transform.position = originPosP2;
    }
}
