using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Warrior : Unit
{
    public UnitData unitData;

    Collider2D enemy;

    private void Awake()
    {
        #region Ω∫≈» √ ±‚»≠
        startHealth = unitData.health;
        damage = unitData.damage;
        defense = unitData.defense;
        attackRange = unitData.attackRange;
        detectRange = unitData.detectRange;
        attackSpeed = unitData.attackSpeed;
        moveSpeed = unitData.moveSpeed;
        owner = PhotonNetwork.IsMasterClient ? EPlayer.Player1 : EPlayer.Player2;
        //this.gameObject.layer = (int)owner;
        #endregion
    }

    private void Update()
    {
        enemy = Physics2D.OverlapCircle(transform.position, attackRange,6);
        if (enemy != null)
        {
            OnAttack(enemy);
        }
    }

    
    protected override void OnAttack(Collider2D enemy)
    {
        enemy.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
    }

    [PunRPC]
    public override void OnDamaged(float _damaged)
    {
        base.OnDamaged(_damaged);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
