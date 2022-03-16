using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buffer : Unit
{
    public Transform allyNexus;
    public Animator animator;
    public GameObject buff;

    Collider[] targetCollider;
    float buffRange;
    float buffDamage = 2f;    // Buff Status Delta

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

    protected new void Update()
    {
        if (GameMgr.blueNexus == false || GameMgr.redNexus == false)
        {
            unitState = EUnitState.Idle;
        }

        switch (unitState)
        {
            case EUnitState.Idle:
                break;
            case EUnitState.Move:
                Move();
                break;
            case EUnitState.Approach:
                Approach();
                break;
            case EUnitState.Attack:
                break;
            case EUnitState.Die:
                Die();
                break;
        }
    }

    #region FSM
    void Move()
    {
        targetCollider = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);
        if (1 == targetCollider.Length)
        {
            unitMove.agent.destination = allyNexus.position;                                    // �������� �Ʊ� �ؼ����� ����
        }
        else    // �ڽ��� ������ �ݶ��̴�
        {
            unitMove.agent.destination = targetCollider[1].transform.position;                  // �������� �Ʊ����� ����
            unitState = EUnitState.Approach;
        }
    }

    void Approach() // �Ʊ����� ����
    {
        targetCollider = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);  // ���� �� �Ʊ� Ž��
        if (1 < targetCollider.Length)                                                                                    // ���� �� �Ʊ��� �ִٸ�
        {
            unitMove.agent.destination = targetCollider[1].transform.position;
        }
        else                                                                                                              // ���� �� �Ʊ��� ���ٸ�
        {
            unitMove.agent.destination = allyNexus.position;                                                              // �Ʊ� �ؼ��� ������ �̵�
            unitState = EUnitState.Move;                                                                                  // ��� �̵�
        }
    }

    public override void Attack()   // ������ ����(����)
    {
        return;
    }

    void Die()    // ���� ���
    {
        isDead = true;
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DieAnimation(this.gameObject));
        StartCoroutine(DieAnimation(buff));
    }
    #endregion


}