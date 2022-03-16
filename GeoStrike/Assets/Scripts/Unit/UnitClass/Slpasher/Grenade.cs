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
    public float damage;

    float speed = 5f;   // 투사체 속도

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

        // 수류탄이 타겟위치 까지가면 폭발
        if (transform.position == targetCollider.transform.position)
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
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 5f, Vector3.zero, 0f, LayerMask.GetMask("Enemy"));
        
        // 감지된 적들에게 데미지
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }

        // 이후 처리
        SetGrenadeActive(false);
    }

    //IEnumerator RotateAnimation(Collider enemy)
    //{
    //    isRotate = true;

    //    Vector3 direct = enemy.transform.position - transform.position;     // 방향을 구함
    //    float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // 두 객체 간의 각을 구함
    //    Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // 최종적으로 회전해야 하는 회전값

    //    while (!Mathf.Approximately(transform.rotation.z, target.z))
    //    {
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 1.5f);

    //        yield return null;
    //    }

    //    isRotate = false;
    //}

    //private void OnTriggerEnter2D(Collider2D enemy)
    //{
    //    if (enemy.gameObject.layer == (int)EPlayer.Enemy)
    //    {
    //        enemy.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
    //        SetGrenadeActive(false);
    //    }
    //}
}
