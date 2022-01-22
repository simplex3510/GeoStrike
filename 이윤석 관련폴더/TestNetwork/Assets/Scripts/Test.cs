using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Test : MonoBehaviourPunCallbacks
{
    public Text text;
    public Text state;
    int currentPlayer = 0;

    void Awake() => Screen.SetResolution(960, 540, false);

    void Update() => state.text = PhotonNetwork.NetworkClientState.ToString();

    void Start()
    {
        print("마스터 서버 접속");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("마스터 서버 접속 완료");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions {MaxPlayers = 2}, null);
    }

    public override void OnJoinedRoom()
    {
        print("룸 생성 완료");
        photonView.RPC("UpdateMember", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void UpdateMember()
    {
        text.text = (++currentPlayer).ToString();
        // text.text = (PhotonNetwork.CountOfPlayersInRooms + 1).ToString();
    }

}
