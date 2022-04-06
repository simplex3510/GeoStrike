using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Tower : MonoBehaviourPun, IDamageable
{
    protected Collider enemyCollider;
    public TowerData initStatus;
    public TowerData deltaStatus;

    private Detector detector;

    public LayerMask opponentLayerMask { get; protected set; }

    #region Tower Data
    public float Health { get; private set; }
    public float Defense { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        detector = GameObject.FindObjectOfType<Detector>();

        if (photonView.IsMine)
        {
            opponentLayerMask = 1 << (int)EPlayer.Enemy;
        }
        else
        {
            opponentLayerMask = 1 << (int)EPlayer.Ally;
        }

        #region Initialize Delta Tower Data
        deltaStatus.health = initStatus.health;
        deltaStatus.defense = initStatus.defense;
        #endregion

        #region Initialize Tower Data
        Health = deltaStatus.health;
        Defense = deltaStatus.defense;
        #endregion

        if (photonView.IsMine)
        {
            gameObject.layer = LayerMask.NameToLayer("Tower");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }

    //void Idle()
    //{
    //    enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, detectRange, opponentLayerMask);
    //    if (enemyColliders.Length == 0)         // 탐지 범위 내에 적이 없어졌다면
    //    {
    //        towerState = ETowerState.Idle;
    //        return;
    //    }
    //    else                                    // 탐지 범위 내에 적이 있고
    //    {
    //        enemyColliders = Physics.OverlapCapsule(transform.position, transform.position, attackRange, opponentLayerMask);
    //        if (enemyColliders.Length != 0)     // 공격 범위 내에 적이 있다면
    //        {
    //            towerState = ETowerState.Attack;
    //            return;
    //        }
    //    }

    //}

    [PunRPC]
    public void OnDamaged(float _damage)
    {
        _damage -= Defense;
        Health -= 0 < _damage ? _damage : 0;
        Debug.Log("라ㅓㅏㅣㄹ덪매ㅑㄻ너야ㅐ럼내ㅑ러ㅑㅐㄴ러냉");

        if (Health <= 0)
        {
            StartCoroutine(DieAnimation());
        }
    }

    [PunRPC]
    public void SetTowerActive(bool _isTrue)
    {
        gameObject.SetActive(_isTrue);
        if(photonView.IsMine)
        {
            photonView.RPC("SetTowerActive", RpcTarget.Others, false);
        }
    }

    protected IEnumerator DieAnimation()
    {
        // 클릭한 오브젝트가 죽을때 인터페이스창 초기화
        if (detector.clickedObject == this.gameObject)
        {
            detector.InitInterface();
        }

        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        var color = spriteRenderer.color;
        while (0 <= color.a)
        {
            color.a -= 1f * Time.deltaTime;
            spriteRenderer.color = color;

            yield return null;
        }

        SetTowerActive(false);
        spriteRenderer.color = Color.white;
    }

    private void OnApplicationQuit()
    {
        #region Return Status Init
        deltaStatus.health = initStatus.health;
        deltaStatus.defense = initStatus.defense;
        #endregion
    }

}