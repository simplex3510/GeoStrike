using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkMgr : MonoBehaviourPunCallbacks
{

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("AstarTest", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("ShooterScene");
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.Instantiate("Prefabs/BlueTeam/Player1_Warrior", new Vector3(-8, 0, 0), Quaternion.identity);
            //PhotonNetwork.Instantiate("Prefabs/Bot", new Vector3(-8, 0, 0), Quaternion.identity);
        }
        //else
        //{
        //    PhotonNetwork.Instantiate("Player2", new Vector3(8, 1, 0), Quaternion.Euler(0f, 0f, 180f));
        //}
    }
}
