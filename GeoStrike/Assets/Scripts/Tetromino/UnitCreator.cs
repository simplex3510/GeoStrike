using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(201)]
public class UnitCreator : MonoBehaviour
{
    [SerializeField] private UnitTileContainer unitTileContainer;
    [SerializeField] private TranslocateField translocateField;

    public Unit unit;
    private Geo geo;

    private void Start()
    {
        if (unitTileContainer == null) { unitTileContainer = GameMgr.instance.grid.GetComponentInChildren<UnitTileContainer>(); }
        if (translocateField == null) { translocateField = unitTileContainer.GetComponent<TranslocateField>(); }
        if (geo == null) { geo = GameMgr.instance.canvas.GetComponentInChildren<Geo>(); }
    }

    public void UnitSpawn()
    {
        if (GameMgr.isMaster)
        {
            for(int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for(int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    if (unitTileContainer.unitTileArr[0, row, column].isEmty)
                    {
                        // ÀÚ¿ø È¹µæ
                        if (unit.unitIdx == 0)
                        {
                            Debug.Log("Get GEO : " + Geo.GEO_SQUARE);
                            geo.DeltaGeo(Geo.GEO_SQUARE);
                            return;
                        }
                        // Unit »ý¼º
                        else
                        {
                            //Unit obj = ObjectPool.instance.allyAndEnemyArr[unit.unitIdx - 1].GetObject();
                            //obj.transform.position = unitTileContainer.unitTileArr[0, row, column].transform.position + Vector3.back;
                            //unitTileContainer.unitTileArr[0, row, column].isEmty = false;

                            //translocateField.unitListP1.Add(obj);
                            //obj.transform.SetParent(translocateField.spawnPosP1.transform);
                            //return;
                        }
                    }
                }
            }
        }
        else
        {
            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    if (unitTileContainer.unitTileArr[1, row, column].isEmty)
                    {
                        // ÀÚ¿ø È¹µæ
                        if (unit.unitIdx == 0)
                        {
                            Debug.Log("Get GEO : " + Geo.GEO_SQUARE);
                            geo.DeltaGeo(Geo.GEO_SQUARE);
                            return;
                        }
                        // Unit »ý¼º
                        else
                        {
                            //Unit obj = ObjectPool.instance.poolArr[unit.unitIdx - 1].GetObject();
                            //obj.transform.position = unitTileContainer.unitTileArr[1, row, column].transform.position + Vector3.back;
                            //unitTileContainer.unitTileArr[1, row, column].isEmty = false;

                            //translocateField.unitListP2.Add(obj);
                            //obj.transform.SetParent(translocateField.spawnPosP2.transform);
                            //return;
                        }
                    }
                }
            }
        }
    }
}
