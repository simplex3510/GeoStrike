using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [Header("Netwrok")]
    public Text stateText;
    public InputField roomName;
    public InputField nickname;
    public Button button;
    public Text readyText;
    public Text roomNameText;
    public Text nicknameText;

    [Header("Quit Check")]
    public Button quitButton;
    public Button undoButton;
    public RectTransform checkPanel;
    public Text checkText;
    public Button yesButton;
    public Button noButton;

    readonly int MAX_PLAYER = 2;
    int readyPlayer = 0;

    #region Network
    void Awake()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            button.interactable = false;
            stateText.text = "Connecting to Server";
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            stateText.text = "Online : Server Connected";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClickJoinOrCreateRoom);
            button.interactable = true;
        }
    }

    public override void OnConnectedToMaster()
    {
        stateText.text = "Online : Server Connected";
        button.onClick.AddListener(OnClickJoinOrCreateRoom);
        button.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        stateText.text = "Offline: Reconnecting to Server";
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
            stateText.text = "Room name is required";
        }
        else
        {
            stateText.text = "Nickname is required.";
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
        button.onClick.AddListener(OnClickReadyCancle);
    }

    public void OnClickReadyCancle()
    {
        photonView.RPC("CurrentReady", RpcTarget.MasterClient, EReadyState.Cancle);
        button.onClick.RemoveListener(OnClickReadyCancle);
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
        readyText.text = $"Ready: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";
    }

    [PunRPC]
    void UpReady()
    {
        readyPlayer++;
        readyText.text = $"Ready: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";

        if(readyPlayer == MAX_PLAYER)
        {
            // LoadingSceneController.LoadScene("GameScene");
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    [PunRPC]
    void DownReady()
    {
        readyPlayer--;
        readyText.text = $"Ready: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";
    }
    #endregion

    #region Title
    public void OnClickCheck()
    {
        StartCoroutine(QuitCheck());
    }

    IEnumerator QuitCheck()
    {
        checkPanel.gameObject.SetActive(true);

        checkText.gameObject.SetActive(true);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);

        float height = 0;
        while(checkPanel.sizeDelta.y <= 249.9f)
        {
            height += Time.deltaTime * 1500f;
            checkPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            yield return null;
        }
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickQuitCancle()
    {
        StartCoroutine(QuitCancle());
    }

    IEnumerator QuitCancle()
    {
        checkText.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        float height = 250;
        while (0.1f <= checkPanel.sizeDelta.y)
        {
            height -= Time.deltaTime * 1500f;
            checkPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            yield return null;
        }

        checkPanel.gameObject.SetActive(false);
    }

    public void OnClickBack()
    {
        if(PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount - 1 == 0)
            {
                PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
            }
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("ConnectScene");
        }

        if(SceneManager.GetActiveScene().name == "ConnectScene")
        {
            return;
        }
    }
    #endregion
}
