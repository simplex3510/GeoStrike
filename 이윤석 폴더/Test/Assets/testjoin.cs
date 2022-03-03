using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class testjoin : MonoBehaviourPunCallbacks
{
    public Transform canvas;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("PlayerM", Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerG", Vector3.zero, Quaternion.identity);
        }
    }
}
