using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Defender : Unit
{
    private AudioSource theAudio;

    [SerializeField] private AudioClip clip;

    public Animator animator;
    public GameObject shild;

    float stunTime = 3f;

    [PunRPC]
    public void OnEnforceStartHealth()
    {
        deltaStatus.Health += 5;
        if (photonView.IsMine)
        {
            photonView.RPC("OnEnforceStartHealth", RpcTarget.Others);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        shild.layer = this.gameObject.layer;

        theAudio = GetComponent<AudioSource>();
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
                animator.SetBool("isMove", true);
                animator.SetBool("isAttack", false);
                break;
            case EUnitState.Approach:
                break;
            case EUnitState.Attack:
                animator.SetBool("isMove", false);
                animator.SetBool("isAttack", true);
                break;
            case EUnitState.Die:
                StartCoroutine(DieAnimation(shild));
                break;
        }
    }

    public override void Attack()   // 적에게 공격
    {
        if (!photonView.IsMine)
        {
            return;
        }

        enemyCollider = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask).Length != 0 ?
                        Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask)[0] :
                        null;

        theAudio.clip = clip;
        theAudio.Play();

        if (enemyCollider != null)
        {
            enemyCollider.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);

            if(enemyCollider.GetComponent<Unit>() != null)
            {
                enemyCollider.GetComponent<Unit>().Stun(stunTime);
            }
        }

        unitMove.SetMove();
        unitState = EUnitState.Move;
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}