using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTileContainer : MonoBehaviour
{
    public List<UnitTile> unitTileList = new List<UnitTile>();
    [SerializeField] private Transform parent;

    private void Awake()
    {
        unitTileList.AddRange(parent.GetComponentsInChildren<UnitTile>());
    }
}
