using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Buffer : Unit
{
    public Transform allyNexus;
    public Animator animator;
    public GameObject buff;

    NavMeshAgent agent;
    Collider[] targetColliders;
    float buffRange;
    float buffDamage = 2f;    // Delta Buff Status 

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
        agent = GetComponent<NavMeshAgent>();
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

        if (GameMgr.blueNexus == false || GameMgr.redNexus == false)
        {
            unitState = EUnitState.Idle;
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
            agent.SetDestination(allyNexus.position);                                  // �������� �Ʊ� �ؼ����� ����
        }
        else                                                                           // �ڽ��� ������ �ݶ��̴�
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
            agent.SetDestination(targetColliders[1].transform.position - new Vector3(0.8f, 0f, 0.8f));                         // �Ʊ����� ����
        }
        else                                                                                                               // ���� �� �Ʊ��� ���ٸ�
        {
            unitState = EUnitState.Move;                                                                                   // �Ʊ� �ؼ����� �̵�
        }
    }

    new void Die()    // ���� ���
    {
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DieAnimation(body));
    }
    #endregion
}