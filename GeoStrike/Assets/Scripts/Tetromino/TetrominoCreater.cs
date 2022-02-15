using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TetrominoCreater : MonoBehaviourPun
{
    public TetrominoTileContainer tileContainer;
    public TetrominoSlotContainer slotContainer;
    public Detector detector;

    private Vector2[] resultTileCoord = new Vector2[4];

    private void Awake()
    {
        if (tileContainer == null) { tileContainer = GameObject.FindObjectOfType<TetrominoTileContainer>(); }
        if (slotContainer == null) { slotContainer = GameObject.FindObjectOfType<TetrominoSlotContainer>(); }
    }

    // Build tetromino
    public void BuildTetromino(GameObject _tetromino, Vector3 _mousePos,  Vector3 _tileCoord, Vector2[] _tetrominoCoord, Vector3 _angle)
    {
        ArrSumOperator(_tileCoord, ArrMultipleOperator(_tetrominoCoord, _angle));

        //if (!CanBuildPreview(resultTileCoord)) { Debug.Log("There is already Building"); return; }

        BuildOnEmptyTile();
        PhotonNetwork.Instantiate(_tetromino.name, _mousePos - Vector3.forward, Quaternion.Euler(_angle));

        TetrominoPreview.instance.ClearPreview();
        //Set_AllRandomSlot();
    }


    // ����� ���� tile ���ѽ�Ű�� - ���콺 ��ǥ, ��Ʈ�ι̳� ��ǥ, ȸ����
    private void BuildOnEmptyTile()
    {
        // ��Ʈ�ι̳� ȸ�� ���� -> ��ǥ �ջ� -> Ÿ�� �������� ���� ����
        SetTileState(resultTileCoord);
    }


    // ���� �������� �̸����� - ���, ȸ��, Idx
    public bool CanBuildPreview(Vector2[] _resultCoord)
    {
        bool[] isEmptyArr = new bool[4];

        // ����� ��ǥ�� isEmpty�� bool Ÿ���� �迭�� �ű��
        for (int idx = 0; idx < _resultCoord.Length; idx++)
        {
            int coordX = (int)_resultCoord[idx].x;
            int coordY = (int)_resultCoord[idx].y;

            //if (PhotonNetwork.IsMasterClient)
            //{
            //    isEmptyArr[idx] = tileContainer.tileArr[ConnectMgr.MASTER_PLAYER, coordY, coordX].isEmty;
            //}
            //else
            //{
            //    isEmptyArr[idx] = tileContainer.tileArr[ConnectMgr.GUEST_PLAYER, coordY, coordX].isEmty;
            //}
        }
        // 4���� �ϳ��� false�� ������ false ��ȯ 
        for (int idx = 0; idx < isEmptyArr.Length; idx++)
        {
            if (isEmptyArr[idx] == false)
            {
                Detector.canBuild = false;
            }
            else
            {
                Detector.canBuild = true;
            }
        }
        return Detector.canBuild;
    }


    // ��Ʈ�ι̳� 4ĭ�� ��ǥ�� Ÿ�� ��ǥ �ջ�
    private Vector2[] ArrSumOperator(Vector3 _tileCoord, Vector2[] multipleTileCoord)
    {
        Vector2 tileCoord = _tileCoord; // ���콺 �������� Vector2�� ��ȯ

        for (int idx = 0; idx < multipleTileCoord.Length; idx++)
        {
            resultTileCoord[idx] = multipleTileCoord[idx] + tileCoord;
        }

        return resultTileCoord;
    }

    // ��Ʈ�ι̳� 4ĭ�� ��ǥ ���Ҽ� ȸ�� ����
    private Vector2[] ArrMultipleOperator(Vector2[] _tetrominoCoord, Vector3 _angle)
    {
        Vector2[] multipleTileCoord = new Vector2[4];

        float a;    // x��ǥ
        float b;    // y��ǥ
        float sin;  // ȸ����
        float cos;  // ȸ����
        float radian = _angle.z * Mathf.Deg2Rad;    // degree�� radian���� ��ȯ

        float real; // �Ǽ�
        float imagin;  // ���

        // ȸ������ 0�̸� ���� ����� ��ǥ�� ����
        if (_angle == Vector3.zero) { return _tetrominoCoord; }
        else
        {
            for (int idx = 0; idx < _tetrominoCoord.Length; idx++)
            {
                // ����
                // (a + bi)(c + di) = (ac - bd) + (ad + bc)i
                // (0 + 1i)(cos90 + iSin90) = (0 + -1) + (0 + 0)i = (-1, 0)

                a = _tetrominoCoord[idx].x;
                b = _tetrominoCoord[idx].y;
                sin = Mathf.Round(Mathf.Sin(radian));
                cos = Mathf.Round(Mathf.Cos(radian));
                real = (a * cos - b * sin);
                imagin = (a * sin + b * cos);

                multipleTileCoord[idx].x = real;
                multipleTileCoord[idx].y = imagin;
            }
            return multipleTileCoord;
        }
    }

    private void SetTileState(Vector2[] _resultTileCoord)
    {
        for (int idx = 0; idx < _resultTileCoord.Length; idx++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                tileContainer.tileArr[ConnectMgr.MASTER_PLAYER, (int)_resultTileCoord[idx].y, (int)_resultTileCoord[idx].x].isEmty = false;
            }
            else
            {
                tileContainer.tileArr[ConnectMgr.GUEST_PLAYER, (int)_resultTileCoord[idx].y, (int)_resultTileCoord[idx].x].isEmty = false;
            }
        }
    }
}
