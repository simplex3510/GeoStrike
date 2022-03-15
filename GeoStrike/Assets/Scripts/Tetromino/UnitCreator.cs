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
            // �ڿ� ȹ��
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
                        // Unit ����
                        if ((int)unitP1.unitIndex != 6)
                        {

                            Unit unit = ObjectPoolMgr.instance.poolArr[(int)unitP1.initStatus.unitIndex].GetObject();    // �� Pool���� �� ���� ������
                            unit.unitCreator = this;
                            unit.StartCoroutine(unit.IdleToMoveCondition());
                            unit.SetFreezeAll();

                            if (spawnPos == Vector3.zero)
                            {
                                unit.transform.position = unitTileContainer.unitTileArr[ConnectMgr.MASTER_PLAYER, row, column].transform.position; // �� ���� Ÿ�Ͽ� ��ġ                          

                                unit.row = row;
                                unit.column = column;

                                unitTileContainer.unitTransformArr[row, column] = unit.transform;
                                spawnPos = unit.transform.position;
                            }
                            else
                            {
                                unit.transform.position = spawnPos;

                                // �������� �̵��� ������ row, column�� ����� Queue���� ���� �����ͼ� ���� Spawn�� ���ֿ� �ο�
                                RowAndColumn rowAndColumn = rowAndColumnQueue.Dequeue();
                                unit.row = rowAndColumn.row;
                                unit.column = rowAndColumn.column;
                                unitTileContainer.unitTransformArr[unit.row, unit.column] = unit.transform;
                            }

                            // ��Ʋ�ʵ�� ���� �̵������ֱ� ���� �۾�
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
            // �ڿ� ȹ��
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
                        // Unit ����
                        if ((int)unitP2.unitIndex != 6)
                        {
                            Unit unit = ObjectPoolMgr.instance.poolArr[(int)unitP2.initStatus.unitIndex].GetObject();    // �� Pool���� �� ���� ������
                            unit.unitCreator = this;
                            unit.StartCoroutine(unit.IdleToMoveCondition());
                            unit.SetFreezeAll();

                            if (spawnPos == Vector3.zero)
                            {
                                unit.transform.position = unitTileContainer.unitTileArr[ConnectMgr.GUEST_PLAYER, row, column].transform.position; // �� ���� Ÿ�Ͽ� ��ġ


                                unit.row = row;
                                unit.column = column;

                                unitTileContainer.unitTransformArr[row, column] = unit.transform;
                                spawnPos = unit.transform.position;
                            }
                            else
                            {
                                unit.transform.position = spawnPos;

                                // �������� �̵��� ������ row, column�� ����� Queue���� ���� �����ͼ� ���� Spawn�� ���ֿ� �ο�
                                RowAndColumn rowAndColumn = rowAndColumnQueue.Dequeue();
                                unit.row = rowAndColumn.row;
                                unit.column = rowAndColumn.column;
                                unitTileContainer.unitTransformArr[unit.row, unit.column] = unit.transform;
                            }

                            // ��Ʋ�ʵ�� ���� �̵������ֱ� ���� �۾�
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
