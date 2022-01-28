using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    public List<TetrominoTile> tileList = new List<TetrominoTile>();
    static public readonly int LIST_WIDTH = 16;
    static public readonly int LIST_HEIGHT = 8;
    static public readonly int LIST_ONE = 1;

    [SerializeField] private GameObject parent;

    private void Awake()
    {
        tileList.AddRange(parent.GetComponentsInChildren<TetrominoTile>());
    }
}
