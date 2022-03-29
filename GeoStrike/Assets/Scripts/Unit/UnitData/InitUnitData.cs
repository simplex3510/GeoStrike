using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InitUnitData", menuName = "Data/Init Unit Data", order = 1)]
public class InitUnitData : ScriptableObject
{
    [Header("<Init Unit Status>")]

    [SerializeField] public EUnitIndex unitIndex;
    [SerializeField] public string unitName;

    public float Health { get { return health; } }
    [SerializeField] protected float health = 0;

    public float Damage { get { return damage; } }
    [SerializeField] protected float damage = 0;

    public float Defense { get { return defense; } }
    [SerializeField] protected float defense = 0;

    public float AttackRange { get { return attackRange; } }
    [SerializeField] protected float attackRange = 0;

    public float DetectRange { get { return detectRange; } }
    [SerializeField] protected float detectRange = 0;

    public float AttackSpeed { get { return attackSpeed; } }
    [SerializeField] protected float attackSpeed = 0;

    public float MoveSpeed { get { return moveSpeed; } }
    [SerializeField] protected float moveSpeed = 0;
}
