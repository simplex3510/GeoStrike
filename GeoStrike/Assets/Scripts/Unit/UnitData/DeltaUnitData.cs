using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeltaUnitData", menuName = "Data/Delta Unit Data", order = 2)]
public class DeltaUnitData : InitUnitData
{
    // [Header("<Delta Unit Status>")]

    public new float Health { get { return health; } set { health = value; } }
    // [SerializeField] protected float health = 0;

    public new float Damage { get { return damage; } set { damage = value; } }
    // [SerializeField] protected float damage = 0;

    public new float Defense { get { return defense; } set { defense = value; } }
    // [SerializeField] protected float defense = 0;

    public new float AttackRange { get { return attackRange; } set { attackRange = value; } }
    // [SerializeField] protected float attackRange = 0;

    public new float DetectRange { get { return detectRange; } set { detectRange = value; } }
    // [SerializeField] protected float detectRange = 0;

    public new float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
    // [SerializeField] protected float attackSpeed = 0;

    public new float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    // [SerializeField] protected float moveSpeed = 0;
}
