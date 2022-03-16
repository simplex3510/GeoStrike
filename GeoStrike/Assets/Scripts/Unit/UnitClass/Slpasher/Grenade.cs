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

    float speed = 5f;   // ����ü �ӵ�

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

        // ����ź�� Ÿ����ġ �������� ����
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
        // ���߹��� �� �� ����
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 5f, Vector3.zero, 0f, LayerMask.GetMask("Enemy"));
        
        // ������ ���鿡�� ������
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
        }

        // ���� ó��
        SetGrenadeActive(false);
    }

    //IEnumerator RotateAnimation(Collider enemy)
    //{
    //    isRotate = true;

    //    Vector3 direct = enemy.transform.position - transform.position;     // ������ ����
    //    float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // �� ��ü ���� ���� ����
    //    Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // ���������� ȸ���ؾ� �ϴ� ȸ����

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
