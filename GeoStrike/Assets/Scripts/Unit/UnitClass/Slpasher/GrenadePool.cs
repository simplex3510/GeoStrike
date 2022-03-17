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

            newGrenade.myPool = grenadeQueue;                     // 투사체 풀(Queue) 설정
            newGrenade.SetGrenadeActive(false);                   // 투사체 비활성화
            newGrenade.transform.SetParent(this.transform);      // 투사체 부모 객체 설정
            return newGrenade;
        }
        else
        {
            return null;
        }
    }

    public Grenade GetGrenade()
    {
        // Pool에 Object가 있을 경우 - 꺼내기
        if (0 < grenadeQueue.Count)
        {
            Grenade newGrenade = grenadeQueue.Dequeue();

            return newGrenade;
        }
        // Pool에 Object가 부족 할 경우 - 새로 만들고 꺼내기
        else
        {
            Grenade newGrenade = CreateGrenade();
            grenadeQueue.Dequeue();

            return newGrenade;
        }
    }
}
