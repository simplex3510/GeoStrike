using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPoolMgr : MonoBehaviourPun
{
    public static ObjectPoolMgr instance;

    public int unitCountx2;

    public PlayerPool[] playerPoolArr = new PlayerPool[2];


    private void Awake()
    {
        instance = this;

        if (photonView.IsMine)
        {
            PhotonNetwork.Instantiate("P1Pool", transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("P2Pool", transform.position, Quaternion.identity);
        }
    }
}
