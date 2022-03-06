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
        // bullet �̸� �޴� ��� ���� �����غ��ñ� ... shooter.bullet.bulletName �����ʿ� <<<
        Bullet newBullet = PhotonNetwork.Instantiate(shooter.bullet.bulletName, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();

        newBullet.queue = bulletQueue;
        newBullet.transform.SetParent(newBullet.transform);
        newBullet.gameObject.SetActive(false);

        return newBullet;
    }

    public Bullet GetBullet()
    {
        // Pool�� Object�� ���� ��� - ������
        if (bulletQueue.Count > 0)
        {
            Bullet newBullet = bulletQueue.Dequeue();
            newBullet.transform.SetParent(null);
            newBullet.SetBulletActive(true);

            return newBullet;
        }
        // Pool�� Object�� ���� �� ��� - ���� ����� ������
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
