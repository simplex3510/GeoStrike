using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class TetrominoTileContainer : MonoBehaviourPun
{
    public const int ARRAY_COLUMN = 16;
    public const int ARRAY_ROW = 8;
    public const int ARRAY_PLAYER = 2;

    public TetrominoTile[,,] tileArr = new TetrominoTile[ARRAY_PLAYER, ARRAY_ROW, ARRAY_COLUMN];

    [SerializeField] private Transform parentP1;
    [SerializeField] private Transform parentP2;


    private void Awake()
    {
        InitTileCoordinate();
    }

    // �迭�� Ÿ�� �ʱ�ȭ, Ÿ�Ͽ� ��ǥ�� �ʱ�ȭ
    private void InitTileCoordinate()
    {
        int idx = 0;

        if (PhotonNetwork.IsMasterClient)
        {
            for (int row = 0; row < ARRAY_ROW; row++)
            {
                for (int column = 0; column < ARRAY_COLUMN; column++)
                {
                    tileArr[0, row, column] = parentP1.transform.GetChild(idx).GetComponent<TetrominoTile>();
                    tileArr[0, row, column].tileCoord = new Vector2((int)column, (int)row); // ���ϴ� ���� �������� ���ʷ� ��ǥ�� �ʱ�ȭ
                    idx++;
                }
            }
        }
        else
        {
            for (int row = 0; row < ARRAY_ROW; row++)
            {
                for (int column = 0; column < ARRAY_COLUMN; column++)
                {
                    tileArr[1, row, column] = parentP2.transform.GetChild(idx).GetComponent<TetrominoTile>();
                    tileArr[1, row, column].tileCoord = new Vector2((int)column, (int)row); // ���ϴ� ���� �������� ���ʷ� ��ǥ�� �ʱ�ȭ
                    idx++;
                }
            }
        }
    }
}
