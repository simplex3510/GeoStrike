using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreation : MonoBehaviour
{
    [SerializeField] private UnitTileContainer unitTileContainer;

    public Unit unit;

    [SerializeField] private int unitIdx; 

    private void Start()
    {
        unitTileContainer = GameMgr.instance.grid.GetComponentInChildren<UnitTileContainer>();
    }

    public void UnitSpawn()
    {
        for(int idx = 0; idx < unitTileContainer.unitTileList.Count; idx++)
        {
            if (unitTileContainer.unitTileList[idx].isEmty)
            { 
                
                Unit obj = ObjectPoolMgr.instance.GetObject(unit, unitIdx);
                obj.transform.position = unitTileContainer.unitTileList[idx].transform.position;
                unitTileContainer.unitTileList[idx].isEmty = false;
                break;
            }
        }
    }
}
