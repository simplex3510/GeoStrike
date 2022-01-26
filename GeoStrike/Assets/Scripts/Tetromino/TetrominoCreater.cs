using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoCreater : MonoBehaviour
{
    public TileContainer m_tileContainer;
    private int m_height { get; set; }

    // Build tetromino
    public void BuildTetromino(GameObject _tetromino, Vector3 _pos, int _rot, int _idx)
    {
        LimitBuildTile(TetrominoPreview.instance.m_clickSlot.m_currentBlockShape, _rot, _idx);
        Instantiate(_tetromino, _pos - Vector3.forward, Quaternion.identity);

        TetrominoPreview.instance.ClearPreview();
        Set_AllRandomSlot();
    }

    // _idx = 마우스 위치의 tile Idx
    private int Get_LimitHeight(int _idx)
    {
        if (_idx < TileContainer.LIST_WIDTH) { m_height = 0; }
        else if (_idx >= TileContainer.LIST_WIDTH && _idx < 2*TileContainer.LIST_WIDTH) { m_height = 1; }
        else if (_idx >= 2*TileContainer.LIST_WIDTH && _idx < 3*TileContainer.LIST_WIDTH) { m_height = 2; }
        else if (_idx >= 3*TileContainer.LIST_WIDTH && _idx < 4*TileContainer.LIST_WIDTH) { m_height = 3; }
        else if (_idx >= 4*TileContainer.LIST_WIDTH && _idx < 5*TileContainer.LIST_WIDTH) { m_height = 4; }
        else if (_idx >= 5*TileContainer.LIST_WIDTH && _idx < 6*TileContainer.LIST_WIDTH) { m_height = 5; }
        else if (_idx >= 6*TileContainer.LIST_WIDTH && _idx < 7*TileContainer.LIST_WIDTH) { m_height = 6; }
        else if (_idx >= 7*TileContainer.LIST_WIDTH && _idx < 8*TileContainer.LIST_WIDTH) { m_height = 7; }

        return m_height;
    }

    // 빌드 가능지역 미리보기 - 모양, 회전, Idx
    public void CanBuildPreview(int _tetromino, int _rot, int _idx)
    {
        Get_LimitHeight(_idx);

        switch (_tetromino)
        {
            case 0: // Square
                // Catch error
                if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height) 
                {
                    TileDetector.m_canBuild = false; 
                    return; 
                }
                else if (_idx - TileContainer.LIST_WIDTH < 0)
                {
                    TileDetector.m_canBuild = false;
                    return;
                }
                // Preview
                if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                     m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                     m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                     m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false)
                {
                    TileDetector.m_canBuild = true;
                }
                else { TileDetector.m_canBuild = false; }
                break;
            case 1: // Straight
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2 * TileContainer.LIST_WIDTH < 0 || _idx - 3 * TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 3 * TileContainer.LIST_WIDTH].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 3*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        { 
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 3 * TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                }
                break;
            case 2: // T
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2 * TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + 2 * TileContainer.LIST_ONE].m_IsBuilding == false) 
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                            _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                }
                break;
            case 3: // L
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + 2 * TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE - 2 * TileContainer.LIST_WIDTH].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                }
                break;
            case 4: // J
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 2:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + 2 * TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 3:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                }
                break;
            case 5: // Z
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2*TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if (m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                }
                break;
            case 6: // S
                switch (_rot)
                {
                    case 0:
                        // Catch error
                        if (_idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0 || _idx - 2 * TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
                        break;
                    case 1:
                        // Catch error
                        if ( _idx == (TileContainer.LIST_WIDTH - TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height ||
                             _idx == (TileContainer.LIST_WIDTH - 2*TileContainer.LIST_ONE) + TileContainer.LIST_WIDTH * m_height)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        else if (_idx - TileContainer.LIST_WIDTH < 0)
                        {
                            TileDetector.m_canBuild = false;
                            return;
                        }
                        // Preview
                        if ( m_tileContainer.m_tileList[_idx].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].m_IsBuilding == false &&
                             m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].m_IsBuilding == false)
                        { TileDetector.m_canBuild = true; }
                        else { TileDetector.m_canBuild = false; }
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
                m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                break;
            // Straight
            case 1:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 2*TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 3*TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 2*TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 3*TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                }
                break;
            // T
            case 2:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 2*TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + 2*TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                    case 2: // Z angle : -180
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                    case 3: // Z angle : -270
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                }
                break;
            // L
            case 3:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 2*TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 2*TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 2*TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                    case 2: // Z angle : -180
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + 2*TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 2*TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                    case 3: // Z angle : -270
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE - 2*TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        break;
                }
                break;
            // J
            case 4:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 2*TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 2*TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 2*TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        break;
                    case 2: // Z angle : -180
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + 2*TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                    case 3: // Z angle : -270
                        m_tileContainer.m_tileList[_idx - 2*TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 2*TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                }
                break;
            // Z
            case 5:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                }
                break;
            // J
            case 6:
                switch (_rot)
                {
                    case 0: // Z angle : 0
                        m_tileContainer.m_tileList[_idx - 2 * TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx - TileContainer.LIST_WIDTH + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        break;
                    case 1: // Z angle : -90
                        m_tileContainer.m_tileList[_idx].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        m_tileContainer.m_tileList[_idx + 2 * TileContainer.LIST_ONE - TileContainer.LIST_WIDTH].m_IsBuilding = true;
                        break;
                }
                break;
        }
    }

    // Set all slot Random
    private void Set_AllRandomSlot()
    {
        foreach(TetrominoSlot slot in GameMgr.instance.m_slotList)
        {
            slot.RandomTetromino();
        }
    }
}
