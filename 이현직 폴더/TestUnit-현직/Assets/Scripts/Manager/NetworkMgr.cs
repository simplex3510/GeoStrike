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
        PhotonNetwork.JoinOrCreateRoom("Test", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Player1", new Vector3(-8, -1, 0), Quaternion.identity);
            PhotonNetwork.Instantiate("Player1_Shooter", new Vector3(-8, -1.5f, 0), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Player2", new Vector3(8, 1, 0), Quaternion.Euler(0f, 0f, 180f));
            PhotonNetwork.Instantiate("Player2_Shooter", new Vector3(8, 1.5f, 0), Quaternion.identity);
        }
    }
}
