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

    [SerializeField] float speed;   // 투사체 속도
    bool isRotate;

    private void Awake()
    {
        //endPosition = startPosition;
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.fixedDeltaTime;    // X축 방향으로 총알을 발사

        // 타겟 좌표까지 이동한 후 투사체 비활성화 - 투사체가 발사된 후 타겟이 비활성화 되었을 때
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

        Vector3 direct = enemy.transform.position - transform.position;     // 방향을 구함
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // 두 객체 간의 각을 구함
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // 최종적으로 회전해야 하는 회전값

        while (!Mathf.Approximately(transform.rotation.z, target.z))
        {
            // delta값 올려주기, 총알 리턴 매소드 추가
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
