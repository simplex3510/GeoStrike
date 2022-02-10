using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslocateField : MonoBehaviour
{
    // Unit state
    public List<Unit> unitList = new List<Unit>();

    // Waiting unit translocate
    [SerializeField] private GameObject moveToBattleField;

    // Waiting unit parent
    public GameObject spawnPos;
    public Vector3 originPos;

    private void Awake()
    {
        originPos = spawnPos.transform.position;
    }

    public void TranslocateUnits() 
    {
        spawnPos.transform.position = moveToBattleField.transform.position;
        ClearList();
    }

    private void ClearList()
    {
        for (int idx = 0; idx < unitList.Count; idx++)
        {
            unitList[idx].transform.parent = null;
        }

        unitList.Clear();
        spawnPos.transform.position = originPos;
    }
}
