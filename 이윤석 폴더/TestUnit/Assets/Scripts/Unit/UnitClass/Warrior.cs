using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warrior : Unit
{
    public UnitData unitData;
    public Animator animator;

    public GameObject weapon;

    protected override void Awake()
    {
        base.Awake();

        #region Initialize
        startHealth = unitData.health;
        damage = unitData.damage;
        defense = unitData.defense;
        attackRange = unitData.attackRange;
        detectRange = unitData.detectRange;
        attackSpeed = unitData.attackSpeed;
        moveSpeed = unitData.moveSpeed;
        #endregion
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        switch (unitState)
        {
            case EUnitState.Idle:
                break;
            case EUnitState.Move:
                animator.SetBool("isMove", true);
                animator.SetBool("isAttack", false);
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                animator.SetBool("isMove", false);
                animator.SetBool("isAttack", true);
                break;
            case EUnitState.Die:
                StartCoroutine(DieAnimation(weapon));
                break;
        }
    }
}