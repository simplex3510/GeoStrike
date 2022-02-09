using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkMgr : MonoBehaviourPunCallbacks
{
    public Text stateText;
    public InputField nickname;
    public Button button;
    public Text readyText;

    readonly int MAX_PLAYER = 2;
    int readyPlayer = 0;

    void Start()
    {
        button.interactable = false;
        stateText.text = "마스터 서버에 접속 중";
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        stateText.text = "온라인 : 마스터 서버 접속 완료";
        button.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        stateText.text = "오프라인 : 마스터 서버 재접속 중";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnClickJoinOrCreateRoom()
    {
        if (PhotonNetwork.IsConnected && nickname.text != "")
        {
            PhotonNetwork.NickName = nickname.text;
            PhotonNetwork.JoinOrCreateRoom("MainGame", new RoomOptions { MaxPlayers = 2 }, null);
        }
        else
        {
            stateText.text = "닉네임 필요";
        }
    }

    public override void OnJoinedRoom()
    {
        nickname.gameObject.SetActive(false);
        readyText.gameObject.SetActive(true);

        readyText.text = $"준비: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";
        button.GetComponentInChildren<Text>().text = "Ready";
    }

    [PunRPC]
    void UpdateReady()
    {
        
    }



}
