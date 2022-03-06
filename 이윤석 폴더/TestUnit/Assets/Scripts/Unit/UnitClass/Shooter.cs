using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooter : Unit
{
    //public Animator animator;
    public Transform[] bulletSpawnPos = new Transform[2];
    int bulletPosIdx = 0;

    public Bullet bullet;


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
                //animator.SetBool("isMove", true);
                //animator.SetBool("isAttack", false);
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                //animator.SetBool("isMove", false);
                //animator.SetBool("isAttack", true);
                Attack();
                break;
            case EUnitState.Die:
                break;
        }
    }

    public override void Attack()
    {
        enemyCollider2D = Physics2D.OverlapCircle(transform.position, attackRange, opponentLayerMask);

        if(isPlayer1)
        {

        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            bulletPosIdx = bulletPosIdx > 0 ? 0 : 1;
            PhotonNetwork.Instantiate("Prefabs/BlueTeam/BuleTeam_Bullet", bulletSpawnPos[bulletPosIdx].position, Quaternion.identity).GetComponent<Bullet>().targetCollider2D = enemyCollider2D;
        }
    }
}
