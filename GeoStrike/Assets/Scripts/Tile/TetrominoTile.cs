using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetrominoTile : MonoBehaviour
{
    public bool isEmty = true;

    public Vector2 tileCoordinate; // ��ǥ ��

    public Vector2 tileCoord { get { return tileCoordinate; } set { tileCoordinate = value; } }
}
