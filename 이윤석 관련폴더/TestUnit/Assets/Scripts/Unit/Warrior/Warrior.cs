using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

        // print($"{gameObject.name} - layer: " + gameObject.layer);
        // print($"{gameObject.name} - enemyLayerMask: " + opponentLayerMask.value);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
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
                //Approach();
                break;
            case EUnitState.Attack:
                animator.SetBool("isMove", false);
                animator.SetBool("isAttack", true);
                //animator.SetTrigger("Attack");
                break;
            case EUnitState.Die:
                //print("Die");
                StartCoroutine(DieAnimation());
                break;
        }
    }

    IEnumerator DieAnimation()
    {
        unitState = EUnitState.Idle;

        var spriteRenderer = weapon.GetComponentInChildren<SpriteRenderer>();
        var color = spriteRenderer.color;
        while (0 <= color.a)
        {
            color.a -= 1f * Time.deltaTime;
            spriteRenderer.color = color;

            yield return null;
        }

        spriteRenderer.color = Color.white;
        gameObject.SetActive(false);
    }
}
