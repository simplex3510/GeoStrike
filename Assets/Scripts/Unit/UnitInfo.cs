using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Create Unit data Object")]
public class UnitInfo : ScriptableObject
{
    [Header("< Main status > ")]
    public string unitName;
    public int hp;
    public int damage;
    public int defense;

    [Header(" < Sub status > ")]
    public float attackRange;
    public float attackSpeed;
    public float movementSpeed;
}
