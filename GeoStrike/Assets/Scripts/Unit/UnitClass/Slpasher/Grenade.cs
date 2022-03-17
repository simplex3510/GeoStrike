using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Grenade : MonoBehaviourPun
{
    // Bullet Pool
    public Queue<Grenade> myPool;

    public Collider targetCollider;
    public Vector3 targetPos;
    public float damage;

    [SerializeField] private ParticleSystem explosionEffect;
    
    float speed = 3f;   // 투사체 속도

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            myPool.Enqueue(this);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // 수류탄이 타겟위치 까지가면 폭발
        if (transform.position == targetPos)
        {
            Explosion();
        }
    }

    [PunRPC]
    public void SetGrenadeActive(bool isTrue)
    {
        this.gameObject.SetActive(isTrue);
        if (photonView.IsMine)
        {
            photonView.RPC("SetGrenadeActive", RpcTarget.Others, isTrue);
        }
    }

    private void Explosion()
    {
        // 폭발범위 내 적 감지
        explosionEffect.Play(true);
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 5f, Vector3.zero, 0f, LayerMask.GetMask("Enemy"));
        
        // 감지된 적들에게 데미지
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }

        // 이후 처리
        SetGrenadeActive(false);
        Debug.Log("Explosion");
    }

}
