using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Data/Unit Data", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("<Unit Status>")]
    public EUnitIndex unitIndex;
    public string unitName;
    public float health = 0;
    public float damage = 0;
    public float defense = 0;
    public float attackRange = 0;
    public float detectRange = 0;
    public float attackSpeed = 0;
    public float moveSpeed = 0;
}