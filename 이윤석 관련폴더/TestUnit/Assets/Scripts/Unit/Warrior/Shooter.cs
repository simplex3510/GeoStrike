using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Shooter : Unit
{
    public UnitData unitData;
    public Animator animator;

    public GameObject bullet;
    public Transform bullet_pos;

    public bool isFireRead;

    Collider2D enemyCollider2D;
    bool isRotate;

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
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                StartCoroutine(Shoot());
                StartCoroutine(RotateAnimation(enemyCollider2D));
                break;
            case EUnitState.Die:
                StartCoroutine(DieAnimation());
                break;
        }
    }

    IEnumerator Shoot()
    {
        if (isFireRead)
        {
            GameObject intantBullet = Instantiate(bullet, bullet_pos.position, bullet_pos.rotation);
            Rigidbody2D bulletRigid = intantBullet.GetComponent<Rigidbody2D>();
            bulletRigid.velocity = bullet_pos.forward * 10;
        }
        isFireRead = false;

        yield return new WaitForSeconds(attackSpeed);
        isFireRead = true;
    }

    IEnumerator DieAnimation()
    {
        unitState = EUnitState.Idle;
        yield return null;
    }
    IEnumerator RotateAnimation(Collider2D enemy)
    {
        isRotate = true;

        Vector3 direct = enemy.transform.position - transform.position;     // 방향을 구함
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // 두 객체 간의 각을 구함
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // 최종적으로 회전해야 하는 회전값

        while (!Mathf.Approximately(transform.rotation.z, target.z))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 0.5f);

            yield return null;
        }

        isRotate = false;
    }
}