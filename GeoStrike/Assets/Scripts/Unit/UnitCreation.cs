using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreation : MonoBehaviour
{
    [SerializeField] private UnitTileContainer unitTileContainer;

    [SerializeField] private GameObject unitPrefab;

    private void Start()
    {
        unitTileContainer = GameMgr.instance.grid.GetComponentInChildren<UnitTileContainer>();
    }

    public void UnitSpawn()
    {
        for(int idx = 0; idx < unitTileContainer.UnitTileList.Count; idx++)
        {
            if (unitTileContainer.UnitTileList[idx].isEmty)
            {
                // Instantiate(unitPrefab, unitTileContainer.UnitTileList[idx].transform.position, Quaternion.identity);
                UnitState obj = Pool.instance.Get_Objeet();
                unitTileContainer.UnitTileList[idx].isEmty = false;
                break;
            }
        }
    }
}
