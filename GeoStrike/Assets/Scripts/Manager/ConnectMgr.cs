using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

enum EReadyState
{
    Enter,
    Ready,
    Cancle
}

public class ConnectMgr : MonoBehaviourPunCallbacks
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
        button.onClick.AddListener(OnClickJoinOrCreateRoom);
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

        photonView.RPC("CurrentReady", RpcTarget.MasterClient, EReadyState.Enter);
        button.GetComponentInChildren<Text>().text = "Ready";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickReady);
    }

    public void OnClickReady()
    {
        button.interactable = false;
        photonView.RPC("CurrentReady", RpcTarget.MasterClient, EReadyState.Ready);
    }

    [PunRPC]
    void CurrentReady(EReadyState state)
    {
        switch (state)
        {
            case EReadyState.Enter:
                photonView.RPC("RefreshReady", RpcTarget.All, readyPlayer);
                break;
            case EReadyState.Ready:
                photonView.RPC("UpReady", RpcTarget.All);
                break;
            case EReadyState.Cancle:
                photonView.RPC("DownReady", RpcTarget.MasterClient);
                break;
        }
    }

    [PunRPC]
    void RefreshReady(int _readyPlayer)
    {
        print("RefreshReady");
        readyPlayer = _readyPlayer;
        readyText.text = $"준비: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";
    }

    [PunRPC]
    void UpReady()
    {
        readyPlayer++;
        readyText.text = $"준비: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";

        if(readyPlayer == MAX_PLAYER)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    [PunRPC]
    void DownReady()
    {
        readyPlayer--;
        readyText.text = $"준비: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";
    }
}
