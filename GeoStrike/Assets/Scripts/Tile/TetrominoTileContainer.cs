using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoTileContainer : MonoBehaviour
{
    public List<TetrominoTile> TetrominotileList = new List<TetrominoTile>();
    static public readonly int LIST_WIDTH = 16;
    static public readonly int LIST_ONE = 1;

    [SerializeField] private GameObject parent;

    private void Awake()
    {
        parent = this.gameObject;
        TetrominotileList.AddRange(parent.GetComponentsInChildren<TetrominoTile>());
    }
}
