using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buffer : Unit
{
    public Transform allyNexus;
    public Animator animator;
    public GameObject buff;

    Collider[] targetColliders;
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
        if(!photonView.IsMine)
        {
            return;
        }

        targetColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);
        if (1 == targetColliders.Length)
        {
            unitMove.agent.SetDestination(allyNexus.position);                                  // �������� �Ʊ� �ؼ����� ����
        }
        else                                                                                    // �ڽ��� ������ �ݶ��̴�
        {
            unitState = EUnitState.Approach;
        }
    }

    void Approach() // �Ʊ����� ����
    {
        if (!photonView.IsMine)
        {
            return;
        }

        targetColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);  // ���� �� �Ʊ� Ž��
        if (1 < targetColliders.Length)                                                                                    // ���� �� �Ʊ��� �ִٸ�
        {
            unitMove.agent.SetDestination(targetColliders[1].transform.position);                                          // �Ʊ����� ����
        }
        else                                                                                                               // ���� �� �Ʊ��� ���ٸ�
        {
            unitState = EUnitState.Move;                                                                                   // �Ʊ� �ؼ����� �̵�
        }
    }

    void Die()    // ���� ���
    {
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DieAnimation(body));
    }
    #endregion


}