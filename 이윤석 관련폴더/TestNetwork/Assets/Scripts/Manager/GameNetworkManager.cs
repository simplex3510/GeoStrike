using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public Text joinPlayerText;
    public Text confirmPlayerText;
    public Button confirmButton;
    public Button attackButton;
    public Button defenseButton;


    [SerializeField] int currentPlayer = 0;
    [SerializeField] int confirm = 0;
    [SerializeField] bool confirmSelf = false;

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
        currentPlayer++;
        print("멤버 업데이트 완료");

        joinPlayerText.text = "참가 인원: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        confirmPlayerText.text = "완료" + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    public void OnClickConfirm()
    {
        photonView.RPC("Confirm", RpcTarget.AllBuffered);
        confirmButton.interactable = false;
    }

    [PunRPC]
    void Confirm()
    {
        confirm++;
        confirmPlayerText.text = "완료 " + confirm.ToString() + "/" + PhotonNetwork.CountOfPlayersInRooms.ToString();
        
        print("준비 완료");
    }

    public void OnClickLeaveRoom()
    {
        photonView.RPC("LeaveRoom", RpcTarget.AllBuffered);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TempLobby");
    }

    // public override void OnDisconnected(DisconnectCause cause) => photonView.RPC("LeaveRoom", RpcTarget.AllBuffered);

    [PunRPC]
    void LeaveRoom()
    {
        print("접속 종료");
        joinPlayerText.text = "참가 인원: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }



    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
}
