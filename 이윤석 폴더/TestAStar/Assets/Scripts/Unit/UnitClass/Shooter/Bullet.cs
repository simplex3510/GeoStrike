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

    float speed = 5f;   // 투사체 속도
    float endDistance;
    bool isRotate;
    [SerializeField] private int returnTime = 2; 

    private void Awake()
    {

    }

    private void OnEnable()
    {   
        //StartCoroutine(EBulletReturn());
    }

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

        transform.position += transform.right * speed * Time.fixedDeltaTime;    // X축 방향으로 투사체를 발사

        // 타겟 벡터 길이까지 이동한 후 투사체 비활성화 - 투사체가 발사된 후 타겟이 비활성화 되었을 때
        if (Mathf.Approximately(targetCollider2D.transform.position.magnitude, transform.position.magnitude))
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

        Vector3 direct = enemy.transform.position - transform.position;     // 방향을 구함
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;      // 두 객체 간의 각을 구함
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);   // 최종적으로 회전해야 하는 회전값

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

    IEnumerator EBulletReturn()
    {
        yield return new WaitForSeconds(returnTime);
        SetBulletActive(false);
    }
}
