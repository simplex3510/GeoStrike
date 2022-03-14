using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buffer : Unit
{
    public Animator animator;
    public GameObject weapon;

    Collider2D[] allyCollider2D;
    Unit ally;
    EBuff currentBuff;
    float buffRange;

    // Buff Status Delta
    float buffDamage = 2f;

    [PunRPC]
    public void OnEnforceStartHealth()
    {
        deltaStatus.health += 5;
        if (photonView.IsMine)
        {
            photonView.RPC("OnEnforceStartHealth", RpcTarget.Others);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        opponentLayerMask = 1 << (int)EPlayer.Ally;
        buffRange = attackRange;
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
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                Buff(currentBuff);
                break;
            case EUnitState.Die:
                break;
        }
    }

    public override void Attack()   // 적에게 공격
    {
        return;
    }

    protected void Buff(EBuff currentBuff)
    {
        allyCollider2D = Physics2D.OverlapCircleAll(transform.position, buffRange, opponentLayerMask);
        foreach (var _allyCollider2D in allyCollider2D)
        {
            ally = _allyCollider2D.GetComponent<Unit>();
            if (ally.unitIndex != EUnitIndex.Buffer || ally.hasBuff)
            {
                continue;
            }

            switch(currentBuff)
            {
                case EBuff.Damage:
                    OnBuff(buffDamage);
                    break;
            }
            // 사거리 벗어나는 경우를 구현하면서 OffBuff를 실행
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}