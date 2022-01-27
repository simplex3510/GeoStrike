using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TempLobby : MonoBehaviourPunCallbacks
{
    public Text currentPlayerText;
    public Text LobbyText;

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Update()
    {
        LobbyText.text = "로비 접속 완료";
        print("접속 인원 업데이트");
        if(PhotonNetwork.InLobby)
        {
            currentPlayerText.text = "접속 인원: " + PhotonNetwork.CountOfPlayers.ToString();
        }
    }

    public void OnClickJoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("Temp", new RoomOptions {MaxPlayers = 2}, null);
    }

    public override void OnJoinedRoom() => PhotonNetwork.LoadLevel("TempRoom");

    public void OnClickDisconnect()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("TempMaster");
    }
}
