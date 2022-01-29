using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TempMaster : MonoBehaviourPunCallbacks
{
    public Text currentStateText;
    public InputField nickname;
    public Button joinLobbyText;

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start()
    {
        currentStateText.text = "마스터 서버에 접속 중";
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster() => currentStateText.text = "온라인 : 마스터 서버 접속 완료";

    public override void OnDisconnected(DisconnectCause cause)
        => PhotonNetwork.ConnectUsingSettings();

    public void OnClickJoinLobby()
    {
        if(PhotonNetwork.IsConnected && nickname.text != "")
        {
            PhotonNetwork.NickName = nickname.text;
            PhotonNetwork.JoinLobby();
        }
        else
        {
            currentStateText.text = $"닉네임 필요";
        }
    }

    public override void OnJoinedLobby() => PhotonNetwork.LoadLevel("TempLobby");
}
