using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(201)]
public class UnitCreator : MonoBehaviourPun
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
        // 磊盔 裙垫
        if (unit.unitIdx == 0)
        {
            Debug.Log("Get GEO : " + Geo.GEO_SQUARE);
            geo.DeltaGeo(Geo.GEO_SQUARE);
            return;
        }
        
        if (photonView.IsMine && GameMgr.isMaster || !photonView.IsMine && !GameMgr.isMaster)
        {
            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    if (unitTileContainer.unitTileArr[ConnectMgr.MASTER_PLAYER, row, column].isEmty)
                    {
                        // Unit 积己
                        if (unit.unitIdx != 0)
                        {
                            Unit obj = ObjectPoolMgr.instance.poolArr[unit.unitIdx - 1].GetObject();


                            obj.transform.position = unitTileContainer.unitTileArr[ConnectMgr.MASTER_PLAYER, row, column].transform.position + Vector3.back;
                            unitTileContainer.unitTileArr[ConnectMgr.MASTER_PLAYER, row, column].isEmty = false;

                            translocateField.p1UnitList.Add(obj);
                            obj.transform.SetParent(translocateField.spawnPosP1.transform);
                            return;
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
                    if (unitTileContainer.unitTileArr[ConnectMgr.GUEST_PLAYER, row, column].isEmty)
                    {
                        // Unit 积己
                        if (unit.unitIdx != 0)
                        {
                            Unit obj = ObjectPoolMgr.instance.poolArr[unit.unitIdx - 1].GetObject();

                            obj.transform.position = unitTileContainer.unitTileArr[ConnectMgr.GUEST_PLAYER, row, column].transform.position + Vector3.back;
                            unitTileContainer.unitTileArr[ConnectMgr.GUEST_PLAYER, row, column].isEmty = false;

                            translocateField.p2UnitList.Add(obj);
                            obj.transform.SetParent(translocateField.spawnPosP2.transform);
                            return;
                            
                        }
                    }
                }
            }
        }
        
    }
}
