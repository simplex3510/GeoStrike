using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoCreater : MonoBehaviour
{
    public TetrominoTileContainer tileContainer;
    public TetrominoSlotContainer slotContainer;

    private int height { get; set; }

    // Temp
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Set_AllRandomSlot();
        }
    }

    // Build tetromino
    public void BuildTetromino(GameObject _tetromino, Vector3 _pos, int _rot, int _idx)
    {
        LimitBuildTile(TetrominoPreview.instance.clickSlot.currentBlockShape, _rot, _idx);
        Instantiate(_tetromino, _pos - Vector3.forward, Quaternion.identity);

        TetrominoPreview.instance.ClearPreview();
        Set_AllRandomSlot();
    }

    // _idx = 마우스 위치의 tile Idx
    private int GetLimitHeight(int _idx)
    {
        if (_idx < TetrominoTileContainer.LIST_WIDTH) { height = 0; }
        else if (_idx >= TetrominoTileContainer.LIST_WIDTH && _idx < 2*TetrominoTileContainer.LIST_WIDTH) { height = 1; }
        else if (_idx >= 2*TetrominoTileContainer.LIST_WIDTH && _idx < 3*TetrominoTileContainer.LIST_WIDTH) { height = 2; }
        else if (_idx >= 3*TetrominoTileContainer.LIST_WIDTH && _idx < 4*TetrominoTileContainer.LIST_WIDTH) { height = 3; }
        else if (_idx >= 4*TetrominoTileContainer.LIST_WIDTH && _idx < 5*TetrominoTileContainer.LIST_WIDTH) { height = 4; }
        else if (_idx >= 5*TetrominoTileContainer.LIST_WIDTH && _idx < 6*TetrominoTileContainer.LIST_WIDTH) { height = 5; }
        else if (_idx >= 6*TetrominoTileContainer.LIST_WIDTH && _idx < 7*TetrominoTileContainer.LIST_WIDTH) { height = 6; }
        else if (_idx >= 7*TetrominoTileContainer.LIST_WIDTH && _idx < 8*TetrominoTileContainer.LIST_WIDTH) { height = 7; }

        return height;
    }

    // 빌드 가능지역 미리보기 - 모양, 회전, Idx
    public void CanBuildPreview(int _tetromino, int _rot, int _idx)
    {
        GetLimitHeight(_idx);

        switch (_tetromino)
        {
            case 0: // Square
                // Catch error
                if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height) 
                {
                    Detector.canBuild = false; 
                    return; 
                }
                else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                {
                    Detector.canBuild = false;
                    return;
                }
                // Preview
                if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                     tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                     tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                     tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true)
                {
                    Detector.canBuild = true;
                }
                else { Detector.canBuild = false; }
                break;
            case 1: // Straight
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2 * TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 3 * TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 3 * TetrominoTileContainer.LIST_WIDTH].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 3*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        { 
                            Detector.canBuild = false;
                            return;
                        }
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 3 * TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                }
                break;
            case 2: // T
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2 * TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true ) 
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2*TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                            _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                }
                break;
            case 3: // L
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if ( _idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2*TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if ( _idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2*TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE - 2 * TetrominoTileContainer.LIST_WIDTH].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                }
                break;
            case 4: // J
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if ( _idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE - TetrominoTileContainer.LIST_WIDTH].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2*TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if ( _idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2*TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                }
                break;
            case 5: // Z
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2*TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if (tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                }
                break;
            case 6: // S
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0 || _idx - 2 * TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TetrominoTileContainer.LIST_WIDTH - TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height ||
                             _idx == (TetrominoTileContainer.LIST_WIDTH - 2*TetrominoTileContainer.LIST_ONE) + TetrominoTileContainer.LIST_WIDTH * height)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        else if (_idx - TetrominoTileContainer.LIST_WIDTH < 0)
                        {
                            Detector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.TetrominotileList[_idx].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE - TetrominoTileContainer.LIST_WIDTH].isEmty == true &&
                             tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE - TetrominoTileContainer.LIST_WIDTH].isEmty == true)
                        { Detector.canBuild = true; }
                        else { Detector.canBuild = false; }
                        break;
                }
                break;

        }
    }

    // 빌드된 지역 tile 제한시키기 - 모양, 회전, Idx
    private void LimitBuildTile(int _tetrimino, int _rot, int _idx)
    {
        switch (_tetrimino)
        {
            // Square
            case 0:
                tileContainer.TetrominotileList[_idx].isEmty = false;
                tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                break;
            // Straight
            case 1:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 2*TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 3*TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 2*TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 3*TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                }
                break;
            // T
            case 2:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 2*TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + 2*TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                    case 2: // Z angle : -180
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                    case 3: // Z angle : -270
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                }
                break;
            // L
            case 3:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 2*TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 2*TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 2*TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                    case 2: // Z angle : -180
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + 2*TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 2*TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                    case 3: // Z angle : -270
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE - 2*TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        break;
                }
                break;
            // J
            case 4:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 2*TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 2*TetrominoTileContainer.LIST_ONE - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 2*TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        break;
                    case 2: // Z angle : -180
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + 2*TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                    case 3: // Z angle : -270
                        tileContainer.TetrominotileList[_idx - 2*TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 2*TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                }
                break;
            // Z
            case 5:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                }
                break;
            // J
            case 6:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.TetrominotileList[_idx - 2 * TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx - TetrominoTileContainer.LIST_WIDTH + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.TetrominotileList[_idx].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE].isEmty = false;
                        tileContainer.TetrominotileList[_idx + TetrominoTileContainer.LIST_ONE - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        tileContainer.TetrominotileList[_idx + 2 * TetrominoTileContainer.LIST_ONE - TetrominoTileContainer.LIST_WIDTH].isEmty = false;
                        break;
                }
                break;
        }
    }

    // Set all slot Random
    private void Set_AllRandomSlot()
    {
        foreach(TetrominoSlot slot in slotContainer.slotList)
        {
            slot.RandomTetromino();
        }
    }
}
