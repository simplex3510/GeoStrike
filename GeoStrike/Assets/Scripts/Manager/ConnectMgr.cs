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
        stateText.text = "������ ������ ���� ��";
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        stateText.text = "�¶��� : ������ ���� ���� �Ϸ�";
        button.onClick.AddListener(OnClickJoinOrCreateRoom);
        button.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        stateText.text = "�������� : ������ ���� ������ ��";
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
            stateText.text = "�г��� �ʿ�";
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
        readyText.text = $"�غ�: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";
    }

    [PunRPC]
    void UpReady()
    {
        readyPlayer++;
        readyText.text = $"�غ�: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";

        if(readyPlayer == MAX_PLAYER)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    [PunRPC]
    void DownReady()
    {
        readyPlayer--;
        readyText.text = $"�غ�: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";
    }
}
