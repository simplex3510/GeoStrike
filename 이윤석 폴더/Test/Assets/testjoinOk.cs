using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class testjoinOk : MonoBehaviourPunCallbacks
{
    public Transform canvas;

   static public  List<RpcTest> pvList = new List<RpcTest>();
    


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
        Vector3 temp= new Vector3();

        if (PhotonNetwork.IsMasterClient)
        {
            temp = new Vector3(360f, 540f, 0);
        }
        else
        {
            temp = new Vector3(1500, 540f, 0);
        }

        PhotonNetwork.Instantiate("TestText", temp, Quaternion.identity);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.Instantiate("PlayerM", temp, Quaternion.identity);
        //}
        //else
        //{
        //    PhotonNetwork.Instantiate("PlayerG", temp, Quaternion.identity);
        //}
    }
}
