using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class BulletPool : MonoBehaviourPun
{
    public Queue<Bullet> bulletQueue = new Queue<Bullet>();
    public Bullet bullet;

    private Bullet CreateBullet()
    {
        if(photonView.IsMine)
        {
            Bullet newBullet = PhotonNetwork.Instantiate("Units/Projectiles/" + bullet.name, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();

            newBullet.myPool = bulletQueue;                     // 투사체 Enqueue
            //newBullet.myParent = this.transform;                // 투사체 부모 객체 설정
            newBullet.transform.SetParent(this.transform);      // 투사체 부모 객체 설정
            newBullet.SetBulletActive(false);                   // 투사체 활성화

            return newBullet;
        }
        else
        {
            return null;
        }
    }

    public Bullet GetBullet()
    {
        // Pool에 Object가 있을 경우 - 꺼내기
        if (0 < bulletQueue.Count)
        {
            Bullet newBullet = bulletQueue.Dequeue();
            //newBullet.transform.SetParent(null);
            newBullet.SetBulletActive(true);

            return newBullet;
        }
        // Pool에 Object가 부족 할 경우 - 새로 만들고 꺼내기
        else
        {
            Bullet newBullet = CreateBullet();
            bulletQueue.Dequeue();
            //newBullet.transform.SetParent(null);
            newBullet.SetBulletActive(true);

            return newBullet;
        }
    }
}
