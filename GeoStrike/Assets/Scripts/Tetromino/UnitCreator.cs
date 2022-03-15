using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(202)]
public class UnitCreator : MonoBehaviourPun
{
    private UnitTileContainer unitTileContainer;
    private TranslocateField translocateField;

    public Queue<RowAndColumn> rowAndColumnQueue = new Queue<RowAndColumn>();
    [HideInInspector] public Vector3 spawnPos = Vector3.zero;

    public Unit unitP1;
    public Unit unitP2;

    private void Start()
    {
        if (unitTileContainer == null) { unitTileContainer = GameMgr.instance.grid.GetComponentInChildren<UnitTileContainer>(); }
        if (translocateField == null) { translocateField = unitTileContainer.GetComponent<TranslocateField>(); }
    }

    public void UnitSpawn()
    {
        if (photonView.IsMine && GameMgr.isMaster)
        {
            // 자원 획득
            if ((int)unitP1.unitIndex == 6)
            {
                Debug.Log("Get GEO : " + Geo.GEO_SQUARE);
                Geo.DeltaGeo(Geo.GEO_SQUARE);
                return;
            }

            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    if (unitTileContainer.unitTransformArr[row, column] == null)
                    {
                        // Unit 생성
                        if ((int)unitP1.unitIndex != 6)
                        {

                            Unit unit = ObjectPoolMgr.instance.poolArr[(int)unitP1.initStatus.unitIndex].GetObject();    // 내 Pool에서 내 유닛 꺼내기
                            unit.unitCreator = this;
                            unit.StartCoroutine(unit.IdleToMoveCondition());
                            unit.SetFreezeAll();

                            if (spawnPos == Vector3.zero)
                            {
                                unit.transform.position = unitTileContainer.unitTileArr[ConnectMgr.MASTER_PLAYER, row, column].transform.position; // 내 유닛 타일에 배치                          

                                unit.row = row;
                                unit.column = column;

                                unitTileContainer.unitTransformArr[row, column] = unit.transform;
                                spawnPos = unit.transform.position;
                            }
                            else
                            {
                                unit.transform.position = spawnPos;

                                // 전장으로 이동된 유닛의 row, column가 저장된 Queue에서 값을 가져와서 새로 Spawn된 유닛에 부여
                                RowAndColumn rowAndColumn = rowAndColumnQueue.Dequeue();
                                unit.row = rowAndColumn.row;
                                unit.column = rowAndColumn.column;
                                unitTileContainer.unitTransformArr[unit.row, unit.column] = unit.transform;
                            }

                            // 배틀필드로 유닛 이동시켜주기 위한 작업
                            translocateField.unitList.Add(unit);
                            unit.transform.SetParent(translocateField.spawnPosP1.transform);
                            return;
                        }

                    }
                }
            }
        }
        else if (photonView.IsMine && !GameMgr.isMaster)
        {
            // 자원 획득
            if ((int)unitP2.unitIndex == 6)
            {
                Debug.Log("Get GEO : " + Geo.GEO_SQUARE);
                Geo.DeltaGeo(Geo.GEO_SQUARE);
                return;
            }

            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    if (unitTileContainer.unitTransformArr[row, column] == null)
                    {
                        // Unit 생성
                        if ((int)unitP2.unitIndex != 6)
                        {
                            Unit unit = ObjectPoolMgr.instance.poolArr[(int)unitP2.initStatus.unitIndex].GetObject();    // 내 Pool에서 내 유닛 꺼내기
                            unit.unitCreator = this;
                            unit.StartCoroutine(unit.IdleToMoveCondition());
                            unit.SetFreezeAll();

                            if (spawnPos == Vector3.zero)
                            {
                                unit.transform.position = unitTileContainer.unitTileArr[ConnectMgr.GUEST_PLAYER, row, column].transform.position; // 내 유닛 타일에 배치


                                unit.row = row;
                                unit.column = column;

                                unitTileContainer.unitTransformArr[row, column] = unit.transform;
                                spawnPos = unit.transform.position;
                            }
                            else
                            {
                                unit.transform.position = spawnPos;

                                // 전장으로 이동된 유닛의 row, column가 저장된 Queue에서 값을 가져와서 새로 Spawn된 유닛에 부여
                                RowAndColumn rowAndColumn = rowAndColumnQueue.Dequeue();
                                unit.row = rowAndColumn.row;
                                unit.column = rowAndColumn.column;
                                unitTileContainer.unitTransformArr[unit.row, unit.column] = unit.transform;
                            }

                            // 배틀필드로 유닛 이동시켜주기 위한 작업
                            translocateField.unitList.Add(unit);
                            unit.transform.SetParent(translocateField.spawnPosP2.transform);
                            return;
                        }
                    }
                }
            }
        }
    }
}
