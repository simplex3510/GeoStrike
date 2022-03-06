using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class BulletPool : MonoBehaviourPun
{
    public Queue<Bullet> bulletQueue = new Queue<Bullet>();

    private Shooter shooter;

    private void Awake()
    {
        shooter = GetComponent<Shooter>();
    }

    private Bullet CreateBullet()
    {
        // bullet 이름 받는 방법 먼저 생각해보시길 ... shooter.bullet.bulletName 수정필요 <<<
        Bullet newBullet = PhotonNetwork.Instantiate(shooter.bullet.bulletName, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();

        newBullet.queue = bulletQueue;
        newBullet.transform.SetParent(newBullet.transform);
        newBullet.gameObject.SetActive(false);

        return newBullet;
    }

    public Bullet GetBullet()
    {
        // Pool에 Object가 있을 경우 - 꺼내기
        if (bulletQueue.Count > 0)
        {
            Bullet newBullet = bulletQueue.Dequeue();
            newBullet.transform.SetParent(null);
            newBullet.SetBulletActive(true);

            return newBullet;
        }
        // Pool에 Object가 부족 할 경우 - 새로 만들고 꺼내기
        else
        {
            Bullet newBullet = CreateBullet();
            bulletQueue.Dequeue();
            newBullet.transform.SetParent(null);
            newBullet.SetBulletActive(true);

            return newBullet;
        }
    }
}
