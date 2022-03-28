
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
    public const int MASTER_PLAYER = 0;
    public const int GUEST_PLAYER = 1;

    public Text stateText;
    public InputField roomName;
    public InputField nickname;
    public Button button;
    public Text readyText;
    public Text roomNameText;
    public Text nicknameText;

    readonly int MAX_PLAYER = 2;
    int readyPlayer = 0;

    void Awake()
    {
        if(!PhotonNetwork.IsConnected)
        {
            button.interactable = false;
            stateText.text = "마스터 서버에 접속 중";
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
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
        if (PhotonNetwork.IsConnected && nickname.text != "" && roomName.text != "")
        {
            PhotonNetwork.NickName = nickname.text;
            PhotonNetwork.JoinOrCreateRoom(roomName.text, new RoomOptions { MaxPlayers = 2 }, null);
        }
        else if (roomName.text == "")
        {
            stateText.text = "룸 이름 필요";
        }
        else
        {
            stateText.text = "닉네임 필요";
        }
    }

    public override void OnJoinedRoom()
    {
        roomNameText.gameObject.SetActive(false);
        roomName.gameObject.SetActive(false);

        nicknameText.gameObject.SetActive(false);
        nickname.gameObject.SetActive(false);

        readyText.gameObject.SetActive(true);

        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -150f);

        photonView.RPC("CurrentReady", RpcTarget.MasterClient, EReadyState.Enter);
        button.GetComponentInChildren<Text>().text = "Ready";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickReady);
    }

    public void OnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("ConnectScene");
    }

    public void OnClickReady()
    {
        photonView.RPC("CurrentReady", RpcTarget.MasterClient, EReadyState.Ready);
        button.onClick.RemoveListener(OnClickReady);
        button.onClick.AddListener(OnClickCancle);
    }

    public void OnClickCancle()
    {
        photonView.RPC("CurrentReady", RpcTarget.MasterClient, EReadyState.Cancle);
        button.onClick.RemoveListener(OnClickCancle);
        button.onClick.AddListener(OnClickReady);
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
                photonView.RPC("DownReady", RpcTarget.All);
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
