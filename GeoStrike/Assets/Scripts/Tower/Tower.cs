using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Tower : MonoBehaviourPun, IDamageable
{
    public TowerData initStatus;
    public TowerData deltaStatus;

    #region Tower Data
    public float Health { get; private set; }
    public float Defense { get; private set; }
    #endregion
    
    protected virtual void Awake()
    {
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
            gameObject.layer = LayerMask.NameToLayer("Ally");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Tower");
        }
    }

    [PunRPC]
    public void OnDamaged(float _damage)
    {
        _damage -= Defense;
        Health -= 0 < _damage ? _damage : 0;

        if (Health <= 0)
        {
            StartCoroutine(DieAnimation());
        }
    }

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