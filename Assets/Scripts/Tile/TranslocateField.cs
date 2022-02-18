using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslocateField : MonoBehaviour
{
    // Unit state
    public List<Unit> unitListP1 = new List<Unit>();
    public List<Unit> unitListP2 = new List<Unit>();

    // Waiting unit translocate
    [SerializeField] private GameObject moveToBattleField;

    // Waiting unit parent
    public GameObject spawnPosP1;
    public GameObject spawnPosP2;
    public Vector3 originPosP1;
    public Vector3 originPosP2;

    private void Awake()
    {
        if (GameMgr.isMaster)
        {
            originPosP1 = spawnPosP1.transform.position;
        }
        else
        {
            originPosP2 = spawnPosP2.transform.position;
        }
    }

    public void TranslocateUnits() 
    {
        if (GameMgr.isMaster)
        {
            spawnPosP1.transform.position = moveToBattleField.transform.position;
        }
        else
        {
            spawnPosP2.transform.position = moveToBattleField.transform.position;
        }
        
        ClearList();
    }

    private void ClearList()
    {
        if (GameMgr.isMaster)
        {
            for (int idx = 0; idx < unitListP1.Count; idx++)
            {
                unitListP1[idx].transform.parent = null;
            }
            unitListP1.Clear();
            spawnPosP1.transform.position = originPosP1;
        }
        else
        {
            for (int idx = 0; idx < unitListP2.Count; idx++)
            {
                unitListP2[idx].transform.parent = null;
            }
            unitListP2.Clear();
            spawnPosP2.transform.position = originPosP2;
        }
    }
}
