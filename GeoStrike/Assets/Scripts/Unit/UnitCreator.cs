using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    [SerializeField] private UnitTileContainer unitTileContainer;

    public Unit unit;
    private Geo geo;

    private void Start()
    {
        if (unitTileContainer == null) { unitTileContainer = GameMgr.instance.grid.GetComponentInChildren<UnitTileContainer>(); }
        if (geo == null) { geo = GameMgr.instance.canvas.GetComponentInChildren<Geo>(); }
    }

    public void UnitSpawn()
    {
        for(int idx = 0; idx < unitTileContainer.unitTileList.Count; idx++)
        {
            if (unitTileContainer.unitTileList[idx].isEmty)
            {
                // ÀÚ¿ø È¹µæ
                if (unit.unitIdx == 0) 
                {
                    Debug.Log("Get GEO : " + Geo.GEO_SQUARE);
                    geo.DeltaGeo(Geo.GEO_SQUARE);
                    break;
                }
                // Unit »ý¼º
                else
                {
                    Unit obj = ObjectPool.instance.poolArr[unit.unitIdx - 1].GetObject();
                    obj.transform.position = unitTileContainer.unitTileList[idx].transform.position;
                    unitTileContainer.unitTileList[idx].isEmty = false;
                    break;
                }
            }
        }
    }
}
