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
        print("������ ���� ����");
        PhotonNetwork.JoinOrCreateRoom("Temp", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        print("�� ����");

        player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);
    }
}
