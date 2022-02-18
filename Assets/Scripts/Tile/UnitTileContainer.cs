using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTileContainer : MonoBehaviour
{
    public static readonly int ARRAY_COLUMN = 16;
    public static readonly int ARRAY_ROW = 8;
    public static readonly int ARRAY_PLAYER = 2;

    public UnitTile[,,] unitTileArr = new UnitTile[ArrayNumber.PLAYER, ArrayNumber.UNIT_TILE_ROW, ArrayNumber.UNIT_TILE_COLUMN];
 
    [SerializeField] private Transform parentP1;
    [SerializeField] private Transform parentP2;


    private void Awake()
    {
        InitTileCoordinate();
        InitTileEnable();
    }

    private void InitTileCoordinate()
    {
        int p1 = 0, p2 = 0;

        for (int player = 0; player < ArrayNumber.PLAYER; player++)
        {
            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    // 0, 1 각각 다른 부모오브젝트 하위로 두기
                    if (player == 0)
                    {
                        unitTileArr[player, row, column] = parentP1.transform.GetChild(p1).GetComponent<UnitTile>();
                        unitTileArr[player, row, column].tileCoord = new Vector2(column, row); // 좌하단 부터 우측으로 차례로 좌표값 초기화
                        p1++;
                    }
                    else
                    {
                        unitTileArr[player, row, column] = parentP2.transform.GetChild(p2).GetComponent<UnitTile>();
                        unitTileArr[player, row, column].tileCoord = new Vector2(column, row); // 좌하단 부터 우측으로 차례로 좌표값 초기화
                        p2++;
                    }
                }
            }
        }
    }

    private void InitTileEnable()
    {
        if (GameMgr.isMaster)
        {
            BoxCollider2D[] boxCollArr = parentP2.GetComponentsInChildren<BoxCollider2D>();
            UnitTile[] tileArr = parentP2.GetComponentsInChildren<UnitTile>();
            for (int idx = 0; idx < boxCollArr.Length; idx++)
            {
                boxCollArr[idx].enabled = false;
                tileArr[idx].enabled = false;
            }
        }
        else
        {
            BoxCollider2D[] boxCollArr = parentP1.GetComponentsInChildren<BoxCollider2D>();
            UnitTile[] tileArr = parentP1.GetComponentsInChildren<UnitTile>();
            for (int idx = 0; idx < boxCollArr.Length; idx++)
            {
                boxCollArr[idx].enabled = false;
                tileArr[idx].enabled = false;
            }
        }
    }

    public void TileAllClear()
    {
        if (GameMgr.isMaster)
        {
            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    unitTileArr[0, row, column].isEmty = true;
                }
            }
        }
        else
        {
            for (int row = 0; row < ArrayNumber.UNIT_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.UNIT_TILE_COLUMN; column++)
                {
                    unitTileArr[1, row, column].isEmty = true;
                }
            }
        }
        
    }
}
