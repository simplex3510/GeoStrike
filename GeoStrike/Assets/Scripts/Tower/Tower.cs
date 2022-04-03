using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum ETowerState
{
    Idle,
    Attack
}

public abstract class Tower : MonoBehaviourPun, IDamageable
{
    public ETowerState towerState;

    protected Collider enemyCollider;
    public TowerData initStatus;
    public TowerData deltaStatus;

    Collider[] enemyColliders;

    private Detector detector;

    public LayerMask opponentLayerMask { get; protected set; }

    #region Tower Data
    public float Health { get; private set; }
    public float Defense { get; private set; }
    public float attackRange { get; protected set; }
    public float attackSpeed { get; protected set; }
    public float damage { get; protected set; }
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
        deltaStatus.attackRange = initStatus.attackRange;
        deltaStatus.attackSpeed = initStatus.attackSpeed;
        deltaStatus.damage = initStatus.damage;
        #endregion

        #region Initialize Tower Data
        Health = deltaStatus.health;
        Defense = deltaStatus.defense;
        attackRange = deltaStatus.attackRange;
        attackSpeed = deltaStatus.attackSpeed;
        damage = deltaStatus.damage;
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

    private void Update()
    {
        switch (towerState)
        {
            case ETowerState.Idle:
                break;
            case ETowerState.Attack:
                break;
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
        deltaStatus.attackRange = initStatus.attackRange;
        deltaStatus.attackSpeed = initStatus.attackSpeed;
        deltaStatus.damage = initStatus.damage;
        #endregion
    }

}