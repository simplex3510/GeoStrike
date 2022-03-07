using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    public Collider2D targetCollider2D;
    public float damage;

    // Bullet Pool
    public Queue<Bullet> myPool;
    public Transform myParent;

    // 이름 초기화
    public string bulletName;


    [SerializeField] float speed;
    LayerMask opponentLayerMask;  // 공격할 대상
    bool isPlayer1;
    bool isRotate;

    private void Awake()
    {
        isPlayer1 = PhotonNetwork.IsMasterClient;
        //endPosition = startPosition;

        if (photonView.IsMine)
        {
            gameObject.layer = (int)EPlayer.Ally;
            opponentLayerMask = 1 << (int)EPlayer.Enemy;
        }
        else
        {
            gameObject.layer = (int)EPlayer.Enemy;
            opponentLayerMask = 1 << (int)EPlayer.Ally;
        }
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

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
            transform.SetParent(myParent);
            myPool.Enqueue(this);
        }
    }

    [PunRPC]
    public void SetBulletActive(bool _bool)
    {
        this.gameObject.SetActive(_bool);

        if (photonView.IsMine)
        {
            photonView.RPC("SetBulletActive", RpcTarget.Others, _bool);
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

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.layer == (int)EPlayer.Enemy)
        {
            enemy.GetComponent<PhotonView>().RPC("OnDamaged", RpcTarget.All, damage);
            SetBulletActive(false);
        }
    }
}
