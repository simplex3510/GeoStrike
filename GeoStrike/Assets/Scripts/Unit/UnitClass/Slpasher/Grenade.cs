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

    public GameObject shootEffect;          // ������ ȿ��
    public GameObject explosionEffect;      // ���� ȿ��

    float speed = 3f;   // ����ü �ӵ�

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

        // ����ź�� Ÿ����ġ �������� ����
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

    [PunRPC]
    public void SetShootActive(bool isTrue)
    {
        shootEffect.SetActive(isTrue);

        if (photonView.IsMine)
        {
            photonView.RPC("SetShootActive", RpcTarget.Others, isTrue);
        }
    }

    [PunRPC]
    public void SetExplosionActive(bool isTrue)
    {
        explosionEffect.SetActive(isTrue);

        if (photonView.IsMine)
        {
            photonView.RPC("SetExplosionActive", RpcTarget.Others, isTrue);
        }
    }

    private void Explosion()
    {
        SetShootActive(false);
        SetExplosionActive(true);
        StartCoroutine(CActiveDelay());

        // ���߹��� �� �� ����
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 5f, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        // ������ ���鿡�� ������
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
            Debug.Log("enemy : " + hitObj.transform.name);
        }
    }

    IEnumerator CActiveDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SetGrenadeActive(false);
    }
}
