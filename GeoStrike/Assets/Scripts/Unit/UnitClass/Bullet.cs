using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    // Bullet Pool
    public Queue<Bullet> myPool;
    public Transform myParent;

    public Collider2D targetCollider2D;
    public float damage;

    [SerializeField] float speed;   // ����ü �ӵ�
    bool isRotate;

    private void Awake()
    {
        //endPosition = startPosition;
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.fixedDeltaTime;    // X�� �������� �Ѿ��� �߻�

        // Ÿ�� ��ǥ���� �̵��� �� ����ü ��Ȱ��ȭ - ����ü�� �߻�� �� Ÿ���� ��Ȱ��ȭ �Ǿ��� ��
        //if(targetCollider2D.transform.position == endPosition)
        //{
        //    gameObject.SetActive(false);
        //}

        if (!isRotate)
        {
            StartCoroutine(RotateAnimation(targetCollider2D));
        }
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            //transform.SetParent(myParent);
            myPool.Enqueue(this);
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
            // delta�� �÷��ֱ�, �Ѿ� ���� �żҵ� �߰�
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 0.5f);

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
