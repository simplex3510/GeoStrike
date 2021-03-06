using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Data/Tower Data", order = 2)]
public class TowerData : ScriptableObject
{
    [Header("<Tower Status>")]
    public string towerName;
    public float health;
    public float defense;
}