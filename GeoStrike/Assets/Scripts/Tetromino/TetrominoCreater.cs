using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoCreater : MonoBehaviour
{
    public TetrominoTileContainer tileContainer;
    public TetrominoSlotContainer slotContainer;

    private int height { get; set; }

    private void Awake()
    {

    }


    // Build tetromino
    public void BuildTetromino(GameObject _tetromino, Vector3 _pos, int _rot, int _idx)
    {
        //BuildOnEmptyTile();
        Instantiate(_tetromino, _pos - Vector3.forward, Quaternion.identity);

        TetrominoPreview.instance.ClearPreview();
        //Set_AllRandomSlot();
    }


    // ���� �������� �̸����� - ���, ȸ��, Idx
    public void CanBuildPreview()
    {
        
        
    }

    // ����� ���� tile ���ѽ�Ű�� - ���, ȸ��, Idx
    private void BuildOnEmptyTile(int _tetrimino, int _rot, int _idx)
    {
       
    }
}
