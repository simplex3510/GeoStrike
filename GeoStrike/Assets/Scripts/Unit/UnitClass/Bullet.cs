using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    public Collider2D targetCollider2D;
    public float damage;

    [SerializeField] float speed;
    bool isPlayer1;
    bool isRotate;


    private void Awake()
    {
        isPlayer1 = PhotonNetwork.IsMasterClient;
        endPosition = startPosition;
    }

    private void Update()
    {
        if (isPlayer1)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }

        // Ÿ�� ��ǥ���� �̵��� �� ����ü ��Ȱ��ȭ - ����ü�� �߻�� �� Ÿ���� ��Ȱ��ȭ �Ǿ��� ��
        if(targetCollider2D.transform.position == endPosition)
        {
            gameObject.SetActive(false);
        }

        if (!isRotate)
        {
            StartCoroutine(RotateAnimation(targetCollider2D));
        }
    }

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.layer == (int)EPlayer.Enemy)
        {
            enemy.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
            gameObject.SetActive(false);
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 0.5f);

            yield return null;
        }

        isRotate = false;
    }

}
