using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TetrominoTileContainer : MonoBehaviour
{
    public static readonly int ARRAY_COLUMN = 16;
    public static readonly int ARRAY_ROW = 8;
    public static readonly int ARRAY_PLAYER = 2;

    public TetrominoTile[,,] tileArr = new TetrominoTile[ARRAY_PLAYER, ARRAY_ROW, ARRAY_COLUMN];

    public static bool isMater = false;

    [SerializeField] private Transform parentP1;
    [SerializeField] private Transform parentP2;

    private void Awake()
    {
        isMater = PhotonNetwork.IsMasterClient;

        InitTileCoordinate();
    }

    // 배열에 타일 초기화, 타일에 좌표값 초기화

    // 방안 1 - 각 플레이어의 idx에만 타일 넣어주기
    private void InitTileCoordinate()
    {
        int p1 = 0, p2 = 0;

        for (int player = 0; player < ARRAY_PLAYER; player++)
        {
            if (isMater == false && player == 0) { continue; }

            for (int row = 0; row < ARRAY_ROW; row++)
            {
                for (int column = 0; column < ARRAY_COLUMN; column++)
                {
                    // 0, 1 각각 다른 부모오브젝트 하위로 두기
                    if (isMater)
                    {
                        tileArr[player, row, column] = parentP1.transform.GetChild(p1).GetComponent<TetrominoTile>();
                        tileArr[player, row, column].tileCoord = new Vector2(column, row); // 좌하단 부터 우측으로 차례로 좌표값 초기화
                        p1++;
                    }
                    else
                    {
                        tileArr[player, row, column] = parentP2.transform.GetChild(p2).GetComponent<TetrominoTile>();
                        tileArr[player, row, column].tileCoord = new Vector2(column, row); // 좌하단 부터 우측으로 차례로 좌표값 초기화
                        p2++;
                    }
                }
            }
        }
    }
}
