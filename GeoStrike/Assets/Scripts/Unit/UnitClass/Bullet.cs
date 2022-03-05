using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Bullet : MonoBehaviour
{
    public Collider2D targetCollider2D;
    bool isRotate;

    [SerializeField] private float speed;

    private void Update()
    {
        if (GameMgr.isMaster)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if(isRotate)
        {
            StartCoroutine(RotateAnimation(targetCollider2D));
        }
    }

    IEnumerator RotateAnimation(Collider2D enemy)
    {
        isRotate = true;

        Vector3 direct = enemy.transform.position - transform.position;     // 방향을 구함
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // 두 객체 간의 각을 구함
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // 최종적으로 회전해야 하는 회전값

        while (!Mathf.Approximately(transform.rotation.z, target.z))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 0.5f);

            yield return null;
        }

        isRotate = false;
    }
}
