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
    public GameObject parent;
    public Vector3 parentOriginPos;

    private void Awake()
    {
        parentOriginPos = parent.transform.position;
    }

    public void TranslocateUnits() 
    {
        parent.transform.position = moveToBattleField.transform.position;
        ClearList();
    }

    private void ClearList()
    {
        for (int idx = 0; idx < unitList.Count; idx++)
        {
            unitList[idx].transform.parent = null;
        }

        unitList.Clear();
        parent.transform.position = parentOriginPos;
    }
}
