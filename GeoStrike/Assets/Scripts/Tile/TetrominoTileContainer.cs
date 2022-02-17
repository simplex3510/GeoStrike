using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoTileContainer : MonoBehaviour
{
    public TetrominoTile[,,] tileArr = new TetrominoTile[ArrayNumber.PLAYER, ArrayNumber.TETROMINO_TILE_ROW, ArrayNumber.TETROMINO_TILE_COLUMN];

    [SerializeField] private Transform parentP1;
    [SerializeField] private Transform parentP2;

    private void Awake()
    {
        InitTileCoordinate();
        InitTileEnable();
    }

    // �迭�� Ÿ�� �ʱ�ȭ, Ÿ�Ͽ� ��ǥ�� �ʱ�ȭ

    // ��� 1 - �� �÷��̾��� idx���� Ÿ�� �־��ֱ�
    private void InitTileCoordinate()
    {
        int p1 = 0, p2 = 0;

        for (int player = 0; player < ArrayNumber.PLAYER; player++)
        {
            for (int row = 0; row < ArrayNumber.TETROMINO_TILE_ROW; row++)
            {
                for (int column = 0; column < ArrayNumber.TETROMINO_TILE_COLUMN; column++)
                {
                    // 0, 1 ���� �ٸ� �θ������Ʈ ������ �α�
                    if (player == 0)
                    {
                        tileArr[player, row, column] = parentP1.transform.GetChild(p1).GetComponent<TetrominoTile>();
                        tileArr[player, row, column].tileCoord = new Vector2(column, row); // ���ϴ� ���� �������� ���ʷ� ��ǥ�� �ʱ�ȭ
                        p1++;
                    }
                    else
                    {
                        tileArr[player, row, column] = parentP2.transform.GetChild(p2).GetComponent<TetrominoTile>();
                        tileArr[player, row, column].tileCoord = new Vector2(column, row); // ���ϴ� ���� �������� ���ʷ� ��ǥ�� �ʱ�ȭ
                        p2++;
                    }
                }
            }
        }
    }

    private void InitTileEnable()
    {
        if (ConnectMgr.isMaster)
        {
            BoxCollider[] boxCollArr = parentP2.GetComponentsInChildren<BoxCollider>();
            TetrominoTile[] tileArr = parentP2.GetComponentsInChildren<TetrominoTile>();
            for (int idx = 0; idx < boxCollArr.Length; idx++)
            {
                boxCollArr[idx].enabled = false;
                tileArr[idx].enabled = false;
            }
        }
        else
        {
            BoxCollider[] boxCollArr = parentP1.GetComponentsInChildren<BoxCollider>();
            TetrominoTile[] tileArr = parentP1.GetComponentsInChildren<TetrominoTile>();
            for (int idx = 0; idx < boxCollArr.Length; idx++)
            {
                boxCollArr[idx].enabled = false;
                tileArr[idx].enabled = false;
            }
        }
    }
}
