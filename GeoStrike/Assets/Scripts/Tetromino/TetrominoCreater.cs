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


    // 빌드 가능지역 미리보기 - 모양, 회전, Idx
    public void CanBuildPreview()
    {
        
        
    }

    // 빌드된 지역 tile 제한시키기 - 모양, 회전, Idx
    private void BuildOnEmptyTile(int _tetrimino, int _rot, int _idx)
    {
       
    }
}
