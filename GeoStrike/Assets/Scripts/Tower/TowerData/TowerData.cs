using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Data/Tower Data", order = 2)]
public class TowerData : ScriptableObject
{
    [Header("<Tower Status>")]
    public float health;
    public float defense;
    public float attackRange;
    public float attackSpeed;
    public float damage;
}