using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TempMaster : MonoBehaviourPunCallbacks
{
    public Text currentStateText;
    public Button joinLobbyText;

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start()
    {
        currentStateText.text = "마스터 서버에 접속 중";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() => currentStateText.text = "온라인 : 마스터 서버 접속 완료";

    public override void OnDisconnected(DisconnectCause cause)
        => PhotonNetwork.ConnectUsingSettings();

    public void OnClickJoinLobby()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby() => PhotonNetwork.LoadLevel("TempLobby");
}
