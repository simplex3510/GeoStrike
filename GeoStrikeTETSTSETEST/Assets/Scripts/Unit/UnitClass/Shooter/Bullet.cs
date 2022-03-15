using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    // Bullet Pool
    public Queue<Bullet> myPool;

    public Collider2D targetCollider2D;
    public float damage;

    float speed = 5f;   // ����ü �ӵ�
    bool isRotate;

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

        //transform.position += transform.right * speed * Time.fixedDeltaTime;    // X�� �������� ����ü�� �߻�
        transform.position = Vector2.MoveTowards(transform.position, targetCollider2D.transform.position, speed * Time.deltaTime);

        // ����ü�� �߻�� �� Ÿ���� ��Ȱ��ȭ �Ǿ��� ��
        if (transform.position == targetCollider2D.transform.position ||
            targetCollider2D.enabled == false)
        {
            SetBulletActive(false);
        }

        if (!isRotate)
        {
            StartCoroutine(RotateAnimation(targetCollider2D));
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

    IEnumerator RotateAnimation(Collider2D enemy)
    {
        isRotate = true;

        Vector3 direct = enemy.transform.position - transform.position;     // ������ ����
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // �� ��ü ���� ���� ����
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // ���������� ȸ���ؾ� �ϴ� ȸ����

        while (!Mathf.Approximately(transform.rotation.z, target.z))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 1.5f);

            yield return null;
        }

        isRotate = false;
    }

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.layer == (int)EPlayer.Enemy)
        {
            enemy.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
            SetBulletActive(false);
        }
    }
}