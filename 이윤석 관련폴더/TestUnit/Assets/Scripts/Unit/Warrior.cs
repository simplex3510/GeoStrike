using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Warrior : Unit
{
    public UnitData unitData;

    Collider2D enemyColider2D;

    private void Awake()
    {
        #region ���� �ʱ�ȭ
        startHealth = unitData.health;
        damage = unitData.damage;
        defense = unitData.defense;
        attackRange = unitData.attackRange;
        detectRange = unitData.detectRange;
        attackSpeed = unitData.attackSpeed;
        moveSpeed = unitData.moveSpeed;
        enemy = PhotonNetwork.IsMasterClient ? EPlayer.Player2 : EPlayer.Player1;
        #endregion
    }

    private void Update()
    {
        enemyColider2D = Physics2D.OverlapCircle(transform.position, attackRange, (int)enemy);
        if (enemyColider2D != null)
        {
            OnAttack(enemyColider2D);
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
