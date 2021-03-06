using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(201)]
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
        // 테트로미노 회전 연산 -> 좌표 합산
        ArrSumOperator(_tileCoord, ArrMultipleOperator(_tetrominoCoord, _angle));

        if (!CanBuildPreview(resultTileCoord)) { Debug.Log("You can`t build there"); return; }

        BuildOnEmptyTile(resultTileCoord);
        if (GameMgr.isMaster)
        {
            PhotonNetwork.Instantiate("Tetromino/Prefabs/BlueTeam/" + _tetromino.name, _mousePos - Vector3.forward, Quaternion.Euler(_angle));
        }
        else
        {
            PhotonNetwork.Instantiate("Tetromino/Prefabs/RedTeam/" + _tetromino.name, _mousePos - Vector3.forward, Quaternion.Euler(_angle));
        }

        TetrominoPreview.instance.ClearPreview();
        ResetTetrominoSlot();
    }


    // 빌드된 지역 tile 제한시키기
    private void BuildOnEmptyTile(Vector2[] _resultTileCoord)
    {
        for (int idx = 0; idx < _resultTileCoord.Length; idx++)
        {
            if (GameMgr.isMaster)
            {
                tileContainer.tileArr[ConnectMgr.MASTER_PLAYER, (int)_resultTileCoord[idx].y, (int)_resultTileCoord[idx].x].isEmty = false;
            }
            else
            {
                tileContainer.tileArr[ConnectMgr.GUEST_PLAYER, (int)_resultTileCoord[idx].y, (int)_resultTileCoord[idx].x].isEmty = false;
            }
        }
    }

    // 건물 생성 후 슬롯 리셋 시키기
    private void ResetTetrominoSlot()
    {
        foreach(TetrominoSlot slot in slotContainer.slotList)
        {
            slot.tetrominoMaker.RandomTetromino();
        }
    }

    // 빌드 가능지역 미리보기 - 모양, 회전, Idx
    public bool CanBuildPreview(Vector2[] _resultCoord)
    {
        bool[] isEmptyArr = new bool[4];

        // 연산된 좌표의 isEmpty를 bool 타입의 배열로 옮기기
        for (int idx = 0; idx < _resultCoord.Length; idx++)
        {
            int coordX = (int)_resultCoord[idx].x;
            int coordY = (int)_resultCoord[idx].y;

            if (coordX < 0 || coordY < 0 || coordX > 15 || coordY > 7) { Detector.canBuild = false; break; }

            if (PhotonNetwork.IsMasterClient)
            {
                isEmptyArr[idx] = tileContainer.tileArr[ConnectMgr.MASTER_PLAYER, coordY, coordX].isEmty;
            }
            else
            {
                isEmptyArr[idx] = tileContainer.tileArr[ConnectMgr.GUEST_PLAYER, coordY, coordX].isEmty;
            }
        }
        // 4개중 하나라도 false가 있으면 false 반환 
        if (isEmptyArr[0] && isEmptyArr[1] && isEmptyArr[2] && isEmptyArr[3]) { return Detector.canBuild = true; }
        else { Detector.canBuild = false; }

        return Detector.canBuild;
    }


    // 테트로미노 4칸의 좌표와 타일 좌표 합산
    private Vector2[] ArrSumOperator(Vector3 _tileCoord, Vector2[] multipleTileCoord)
    {
        Vector2 tileCoord = _tileCoord; // 마우스 포지션을 Vector2로 변환

        for (int idx = 0; idx < multipleTileCoord.Length; idx++)
        {
            resultTileCoord[idx] = multipleTileCoord[idx] + tileCoord;
        }

        return resultTileCoord;
    }

    // 테트로미노 4칸의 좌표와 복소수 회전 연산
    private Vector2[] ArrMultipleOperator(Vector2[] _tetrominoCoord, Vector3 _angle)
    {
        Vector2[] multipleTileCoord = new Vector2[4];

        float a;    // x좌표
        float b;    // y좌표
        float sin;  // 회전각
        float cos;  // 회전각
        float radian = _angle.z * Mathf.Deg2Rad;    // degree를 radian으로 변환

        float real; // 실수
        float imagin;  // 허수

        // 회전각이 0이면 덧셈 연산된 좌표를 전달
        if (_angle == Vector3.zero) { return _tetrominoCoord; }

        // 회전각이 0이 아닐경우 복소수 곱연산 적용
        else
        {
            for (int idx = 0; idx < _tetrominoCoord.Length; idx++)
            {
                // 계산식
                // (a + bi)(c + di) = (ac - bd) + (ad + bc)i
                // a = 0, b = 1, radian = 90 ->(0 + 1i)(cos90 + iSin90) = (0 - 1) + (0 + 0)i = (-1, 0)

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
}
