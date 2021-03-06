using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Create Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("<Stat>")]
    public float health = 0;
    public float damage = 0;
    public float defense = 0;
    public float attackRange = 0;
    public float detectRange = 0;
    public float attackSpeed = 0;
    public float moveSpeed = 0;
}
