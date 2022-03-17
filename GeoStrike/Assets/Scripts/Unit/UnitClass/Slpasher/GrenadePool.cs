using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GrenadePool : MonoBehaviourPun
{
    public Queue<Grenade> grenadeQueue = new Queue<Grenade>();
    public Grenade grenade;

    private Grenade CreateGrenade()
    {
        if (photonView.IsMine)
        {
            Grenade newGrenade = PhotonNetwork.Instantiate("Units/Projectiles/" + grenade.name, Vector3.zero, Quaternion.Euler(90f, 0f, 0f)).GetComponent<Grenade>();

            newGrenade.myPool = grenadeQueue;                     // ����ü Ǯ(Queue) ����
            newGrenade.SetGrenadeActive(false);                   // ����ü ��Ȱ��ȭ
            newGrenade.transform.SetParent(this.transform);      // ����ü �θ� ��ü ����
            return newGrenade;
        }
        else
        {
            return null;
        }
    }

    public Grenade GetGrenade()
    {
        // Pool�� Object�� ���� ��� - ������
        if (0 < grenadeQueue.Count)
        {
            Grenade newGrenade = grenadeQueue.Dequeue();

            return newGrenade;
        }
        // Pool�� Object�� ���� �� ��� - ���� ����� ������
        else
        {
            Grenade newGrenade = CreateGrenade();
            grenadeQueue.Dequeue();

            return newGrenade;
        }
    }
}
