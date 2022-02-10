using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public UnitTileContainer unitTileContainer;
    public TetrominoTileContainer tetrominoTileContainer;

    private void Awake()
    {
        if (unitTileContainer == null) { unitTileContainer = GetComponentInChildren<UnitTileContainer>(); }
        if (tetrominoTileContainer == null) { tetrominoTileContainer = GetComponentInChildren<TetrominoTileContainer>(); }
    }
}
