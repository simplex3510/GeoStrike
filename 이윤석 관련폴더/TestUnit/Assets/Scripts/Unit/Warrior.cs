using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Warrior : Unit
{
    public UnitData unitData;

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

    // protected override void Update()
    // {
    //     base.Update();
    // }

    
    // protected override void Attack(Collider2D enemy)
    // {
    //     enemy.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
    // }

    // [PunRPC]
    // public override void OnDamaged(float _damaged)
    // {
    //     base.OnDamaged(_damaged);
    // }
}
