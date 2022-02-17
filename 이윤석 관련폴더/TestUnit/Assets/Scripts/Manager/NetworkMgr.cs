using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkMgr : MonoBehaviourPunCallbacks
{
    public Unit player;
    public Unit enemy;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("마스터 서버 연결");
        PhotonNetwork.JoinOrCreateRoom("Temp", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        print("룸 연결");

        player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);
    }
}
