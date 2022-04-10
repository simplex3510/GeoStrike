using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    // Bullet Pool
    public Queue<Bullet> myPool;

    public Collider targetCollider;
    public float damage;

    float speed = 7f;   // ����ü �ӵ�

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            myPool.Enqueue(this);
        }
    }

    private void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetCollider.transform.position, speed * Time.deltaTime);

        // ����ü�� �߻�� �� Ÿ���� ��Ȱ��ȭ �Ǿ��� ��
        if (transform.position == targetCollider.transform.position ||
            targetCollider.gameObject.activeSelf == false)
        {
            SetBulletActive(false);
        }
    }

    [PunRPC]
    public void SetBulletActive(bool isTrue)
    {
        this.gameObject.SetActive(isTrue);
        if (photonView.IsMine)
        {
            photonView.RPC("SetBulletActive", RpcTarget.Others, isTrue);
        }
    }

    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.layer == (int)EPlayer.Enemy)
        {
            enemy.gameObject.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
            SetBulletActive(false);
        }
    }
}
