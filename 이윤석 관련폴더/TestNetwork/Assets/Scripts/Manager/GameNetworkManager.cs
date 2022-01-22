using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public Text roomInfoText;

    int currentPlayer = 0;

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start()
    {
        print("마스터 서버 접속 중");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("마스터 서버 접속 완료");
        print("룸 접속 중");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions {MaxPlayers = 2}, null);
    }

    public override void OnJoinedRoom()
    {
        print("룸 접속 완료");
        print("멤버 업데이트 시작");
        photonView.RPC("UpdateMember", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void UpdateMember()
    {
        print("멤버 업데이트 완료");
        roomInfoText.text = "완료 0/" + (++currentPlayer).ToString();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }
}
