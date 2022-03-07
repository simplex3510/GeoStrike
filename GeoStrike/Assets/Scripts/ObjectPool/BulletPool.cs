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

            newBullet.myPool = bulletQueue;                     // ����ü Enqueue
            //newBullet.myParent = this.transform;                // ����ü �θ� ��ü ����
            newBullet.transform.SetParent(this.transform);      // ����ü �θ� ��ü ����
            newBullet.SetBulletActive(false);                   // ����ü Ȱ��ȭ

            return newBullet;
        }
        else
        {
            return null;
        }
    }

    public Bullet GetBullet()
    {
        // Pool�� Object�� ���� ��� - ������
        if (0 < bulletQueue.Count)
        {
            Bullet newBullet = bulletQueue.Dequeue();
            //newBullet.transform.SetParent(null);
            newBullet.SetBulletActive(true);

            return newBullet;
        }
        // Pool�� Object�� ���� �� ��� - ���� ����� ������
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
