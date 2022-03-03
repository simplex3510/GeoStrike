using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(202)]
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
        if (photonView.IsMine && GameMgr.isMaster)
        {
            // �ڿ� ȹ��
            if (unit.unitIdx == 0)
            {
                Debug.Log("Get GEO : " + Geo.GEO_SQUARE);
                geo.DeltaGeo(Geo.GEO_SQUARE);
                return;
            }

            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    if (unitTileContainer.unitTileArr[ConnectMgr.MASTER_PLAYER, row, column].isEmty)
                    {
                        // Unit ����
                        if (unit.unitIdx != 0)
                        {
                            Unit obj = ObjectPoolMgr.instance.poolArr[unit.unitIdx - 1].GetObject();    // �� Pool���� �� ���� ������

                            obj.transform.position = unitTileContainer.unitTileArr[ConnectMgr.MASTER_PLAYER, row, column].worldPos + Vector3.back; // �� ���� Ÿ�Ͽ� ��ġ
                            Debug.Log("after  : " + obj.transform.position);
                            unitTileContainer.unitTileArr[ConnectMgr.MASTER_PLAYER, row, column].isEmty = false;   // ��ȯ�� ���� Ÿ���� ���� ��ȯ
                            
                            // ��Ʋ�ʵ�� ���� �̵������ֱ� ���� �۾�
                            translocateField.unitList.Add(obj);
                            obj.transform.SetParent(translocateField.spawnPosP1.transform); // <- x
                            return;
                        }
                    }
                }
            }
        }
        else if (photonView.IsMine && !GameMgr.isMaster)
        {
            // �ڿ� ȹ��
            if (unit.unitIdx == 0)
            {
                Debug.Log("Get GEO : " + Geo.GEO_SQUARE);
                geo.DeltaGeo(Geo.GEO_SQUARE);
                return;
            }

            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    if (unitTileContainer.unitTileArr[ConnectMgr.GUEST_PLAYER, row, column].isEmty)
                    {
                        // Unit ����
                        if (unit.unitIdx != 0)
                        {
                            Unit obj = ObjectPoolMgr.instance.poolArr[unit.unitIdx - 1].GetObject();    // �� Pool���� �� ���� ������

                            obj.transform.position = unitTileContainer.unitTileArr[ConnectMgr.GUEST_PLAYER, row, column].worldPos + Vector3.back; // �� ���� Ÿ�Ͽ� ��ġ
                            unitTileContainer.unitTileArr[ConnectMgr.GUEST_PLAYER, row, column].isEmty = false;   // ��ȯ�� ���� Ÿ���� ���� ��ȯ

                            // ��Ʋ�ʵ�� ���� �̵������ֱ� ���� �۾�
                            translocateField.unitList.Add(obj);
                            obj.transform.SetParent(translocateField.spawnPosP2.transform);
                            return;
                        }
                    }
                }
            }
        }
    }
}
