using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoCreater : MonoBehaviour
{
    public TileContainer tileContainer;
    public TetrominoSlotContainer slotContainer;

    private int height { get; set; }

    // Build tetromino
    public void BuildTetromino(GameObject _tetromino, Vector3 _pos, int _rot, int _idx)
    {
        LimitBuildTile(TetrominoPreview.instance.clickSlot.currentBlockShape, _rot, _idx);
        Instantiate(_tetromino, _pos - Vector3.forward, Quaternion.identity);

        TetrominoPreview.instance.ClearPreview();
        Set_AllRandomSlot();
    }

    // _idx = 마우스 위치의 tile Idx
    private int Get_LimitHeight(int _idx)
    {
        if (_idx < TileContainer.LIST_WIDTH) { height = 0; }
        else if (_idx >= TileContainer.LIST_WIDTH && _idx < 2*TileContainer.LIST_WIDTH) { height = 1; }
        else if (_idx >= 2*TileContainer.LIST_WIDTH && _idx < 3*TileContainer.LIST_WIDTH) { height = 2; }
        else if (_idx >= 3*TileContainer.LIST_WIDTH && _idx < 4*TileContainer.LIST_WIDTH) { height = 3; }
        else if (_idx >= 4*TileContainer.LIST_WIDTH && _idx < 5*TileContainer.LIST_WIDTH) { height = 4; }
        else if (_idx >= 5*TileContainer.LIST_WIDTH && _idx < 6*TileContainer.LIST_WIDTH) { height = 5; }
        else if (_idx >= 6*TileContainer.LIST_WIDTH && _idx < 7*TileContainer.LIST_WIDTH) { height = 6; }
        else if (_idx >= 7*TileContainer.LIST_WIDTH && _idx < 8*TileContainer.LIST_WIDTH) { height = 7; }

        return height;
    }

    // 빌드 가능지역 미리보기 - 모양, 회전, Idx
    public void CanBuildPreview(int _tetromino, int _rot, int _idx)
    {
        Get_LimitHeight(_idx);

        switch (_tetromino)
        {
            case 0: // Square
                // Catch error
                if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height) 
                {
                    TileDetector.canBuild = false; 
                    return; 
                }
                else if (_idx - TileContainer.LIST_WIDTH < 0)
                {
                    TileDetector.canBuild = false;
                    return;
                }
                // Preview
                if ( tileContainer.tileList[_idx].isBuilding == false &&
                     tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                     tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                     tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false)
                {
                    TileDetector.canBuild = true;
                }
                else { TileDetector.canBuild = false; }
                break;
            case 1: // Straight
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2 * TileContainer.LIST_WIDTH < 0 || _idx - 3 * TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - 3 * TileContainer.LIST_WIDTH].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 3*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        { 
                            TileDetector.canBuild = false;
                            return;
                        }
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + 3 * TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                }
                break;
            case 2: // T
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2 * TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + 2 * TileContainer.LIST_ONE].isBuilding == false) 
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                            _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                }
                break;
            case 3: // L
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + 2 * TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE - 2 * TileContainer.LIST_WIDTH].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                }
                break;
            case 4: // J
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + 2 * TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                }
                break;
            case 5: // Z
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if (tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                }
                break;
            case 6: // S
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2 * TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * height)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.canBuild = false;
                            return;
                        }
                        // Preview
                        if ( tileContainer.tileList[_idx].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding == false &&
                             tileContainer.tileList[_idx + TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].isBuilding == false &&
                             tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].isBuilding == false)
                        { TileDetector.canBuild = true; }
                        else { TileDetector.canBuild = false; }
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
                tileContainer.tileList[_idx].isBuilding = true;
                tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                break;
            // Straight
            case 1:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - 2*TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - 3*TileContainer.LIST_WIDTH].isBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + 2*TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + 3*TileContainer.LIST_ONE].isBuilding = true;
                        break;
                }
                break;
            // T
            case 2:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - 2*TileContainer.LIST_WIDTH].isBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + 2*TileContainer.LIST_ONE].isBuilding = true;
                        break;
                    case 2: // Z angle : -180
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        break;
                    case 3: // Z angle : -270
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE].isBuilding = true;
                        break;
                }
                break;
            // L
            case 3:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + 2*TileContainer.LIST_ONE].isBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - 2*TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - 2*TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        break;
                    case 2: // Z angle : -180
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + 2*TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + 2*TileContainer.LIST_ONE].isBuilding = true;
                        break;
                    case 3: // Z angle : -270
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE - 2*TileContainer.LIST_WIDTH].isBuilding = true;
                        break;
                }
                break;
            // J
            case 4:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + 2*TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + 2*TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].isBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - 2*TileContainer.LIST_WIDTH].isBuilding = true;
                        break;
                    case 2: // Z angle : -180
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + 2*TileContainer.LIST_ONE].isBuilding = true;
                        break;
                    case 3: // Z angle : -270
                        tileContainer.tileList[_idx - 2*TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - 2*TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        break;
                }
                break;
            // Z
            case 5:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE].isBuilding = true;
                        break;
                }
                break;
            // J
            case 6:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        tileContainer.tileList[_idx - 2 * TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        tileContainer.tileList[_idx].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE].isBuilding = true;
                        tileContainer.tileList[_idx + TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].isBuilding = true;
                        tileContainer.tileList[_idx + 2 * TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].isBuilding = true;
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
