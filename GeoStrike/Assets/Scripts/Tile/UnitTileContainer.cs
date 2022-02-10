using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTileContainer : MonoBehaviour
{
    public List<UnitTile> unitTileList = new List<UnitTile>();
    [SerializeField] private Transform parent;

    private void Awake()
    {
        parent = this.transform;
        unitTileList.AddRange(parent.GetComponentsInChildren<UnitTile>());
    }

    public void TileAllClear()
    {
        for(int idx = 0; idx < unitTileList.Count; idx++)
        {
            unitTileList[idx].isEmty = true;
        }
    }
}
