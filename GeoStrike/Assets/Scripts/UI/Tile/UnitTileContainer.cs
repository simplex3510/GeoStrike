using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTileContainer : MonoBehaviour
{
    public List<UnitTile> UnitTileList = new List<UnitTile>();
    [SerializeField] private Transform parent;

    private void Awake()
    {
        UnitTileList.AddRange(parent.GetComponentsInChildren<UnitTile>());
    }
}
