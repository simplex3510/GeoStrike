using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(201)]
public class TranslocateField : MonoBehaviourPun
{
    // Unit
    public List<Unit> unitList = new List<Unit>();
    private UnitTileContainer unitTileContainer;

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
        if (unitTileContainer == null) { unitTileContainer = GameObject.FindObjectOfType<UnitTileContainer>(); }
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
        for (int idx = 0; idx < unitList.Count; idx++)
        {
            unitList[idx].unitCreator.rowAndColumnQueue.Enqueue(unitList[idx].rowAndColumn);    // 배치 자리 기억
            unitTileContainer.unitTransformArr[unitList[idx].row, unitList[idx].column] = null; // 해당 자리의 빈 자리 체크
            if(unitList[idx].GetComponent<Buffer>() != null)
            {
                continue;
            }
            unitList[idx].transform.parent = null;
            unitList[idx].GetComponent<NavMeshAgent>().enabled = true;
            unitList[idx].GetComponent<UnitMove>().enabled = true;
            unitList[idx].GetComponent<UnitMove>().SetMove();
        }
        unitList.Clear();

        spawnPosP1.transform.position = originPosP1;
        spawnPosP2.transform.position = originPosP2;
    }
}
