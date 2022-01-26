using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    public List<TetrominoTile> m_tileList = new List<TetrominoTile>();
    static public readonly int LIST_WIDTH = 16;
    static public readonly int LIST_HEIGHT = 8;
    static public readonly int LIST_ONE = 1;

    [SerializeField] private GameObject m_parent;

    private void Awake()
    {
        m_tileList.AddRange(m_parent.GetComponentsInChildren<TetrominoTile>());
    }
}
