using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

enum E_Btn
{
    CONNECT = 0,
    DISCONNECT,
    JOIN_LOBBY,
    JOIN_ROOM,
    CREATE_ROOM,
    JOIN_AND_CREATE,
    JOIN_RANDOM_ROOM,
    LEAVE_ROOM
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text connectionInfoText;
    public InputField nickName, roomName;
    public Button[] buttons;

    string gameVersion = "1";

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        buttons[0].interactable = true;
        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }

    // void Update() => connectionInfoText.text = PhotonNetwork.NetworkClientState.ToString();


    void Connect()
    {
        buttons[0].interactable = false;

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        connectionInfoText.text = "마스터 서버에 접속 중...";
    }
    public override void OnConnectedToMaster()
    {
        buttons[0].interactable = false;
        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
        PhotonNetwork.LocalPlayer.NickName = nickName.text;
    }


    public void Disconnet() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        buttons[0].interactable = true;
        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음";
    }


    public void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "로비에 접속...";
            PhotonNetwork.JoinLobby();
        }
        else
        {
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n 접속 재시도 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnJoinedLobby() => connectionInfoText.text = "로비 접속 완료";


    public void CreateRoom() => PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 2 });

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomName.text);

    public void JoinOrCreateRoom()
        => PhotonNetwork.JoinOrCreateRoom(roomName.text, new RoomOptions { MaxPlayers = 2 }, null);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom() => connectionInfoText.text = "방 만들기 완료";

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "방 참가 완료";
        PhotonNetwork.LoadLevel("MainGame");
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => connectionInfoText.text = "방 만들기 실패";

    public override void OnJoinRoomFailed(short returnCode, string message) => connectionInfoText.text = "방 참가 실패";

    public override void OnJoinRandomFailed(short returnCode, string message) => connectionInfoText.text = "방 랜덤 참가 실패";


    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("마스터 서버에 연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
}
