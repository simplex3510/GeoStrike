using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

enum EPlayerState
{
    ENTER,
    READY,
    LEAVE,
    CONFIRM
}

public class TempRoom : MonoBehaviourPun
{
    #region UI Field
    public RectTransform gridPanel;
    public Text joinMemberText;
    public Text readyPlayerText;
    public Button readyButton;

    public GameObject gameUIPrefab;
    #endregion 

    [SerializeField] int readyPlayer = 0;
    [SerializeField] int confirmPlayer = 0;

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start()
    {
        tempGame = new TempGame();
        photonView.RPC("UpdateRoom", RpcTarget.MasterClient, EPlayerState.ENTER, !readyButton.interactable);
    }

    public void OnClickBack()
    {
        photonView.RPC("UpdateRoom", RpcTarget.MasterClient, EPlayerState.LEAVE, !readyButton.interactable);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TempMaster");
    }

    public void OnClickReady()
    {
        readyButton.interactable = false;
        photonView.RPC("UpdateRoom", RpcTarget.MasterClient, EPlayerState.READY, !readyButton.interactable);
    }

    public void OnClickConfirm()
    {
        readyButton.interactable = false;
        photonView.RPC("UpdateRoom", RpcTarget.All, EPlayerState.CONFIRM , !readyButton.interactable);
    }

    [PunRPC]
    void UpdateRoom(EPlayerState ePlayerState, bool _isCheck)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(ePlayerState == EPlayerState.ENTER)
            {
                photonView.RPC("UpdatePlayer", RpcTarget.All, readyPlayer, false);
            }
            else if (ePlayerState == EPlayerState.READY)
            {
                photonView.RPC("UpdatePlayer", RpcTarget.All, readyPlayer, true);
            }
            else if (ePlayerState == EPlayerState.CONFIRM)
            {
                photonView.RPC("UpdateConfirm", RpcTarget.All, confirmPlayer, _isCheck);
            }
            else if (ePlayerState == EPlayerState.LEAVE)
            {
                photonView.RPC("UpdatePlayer", RpcTarget.All, _isCheck);
            }
        }
    }

    [PunRPC]
    void UpdatePlayer(int _readyPlayer, bool _isCheck)
    {
        readyPlayer = _readyPlayer;
        if (_isCheck)
        {
            readyPlayer++;
        }

        joinMemberText.text = $"참가 인원: {PhotonNetwork.CurrentRoom.PlayerCount.ToString()}";
        readyPlayerText.text = $"준비: {readyPlayer.ToString()} / {PhotonNetwork.CurrentRoom.MaxPlayers.ToString()}";

        if (readyPlayer == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            joinMemberText.gameObject.SetActive(false);
            readyPlayerText.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(false);

            if (photonView.IsMine)
            {
                Instantiate(gameUIPrefab, Vector3.zero, Quaternion.identity).transform.SetParent(gridPanel.transform);
            }
        }
    }

    [PunRPC]
    void UpdatePlayer(bool _isCheck)
    {
        if(_isCheck)
        {
            readyPlayer--;
        }

        joinMemberText.text = $"참가 인원: {(PhotonNetwork.CurrentRoom.PlayerCount - 1).ToString()}";
        readyPlayerText.text = $"준비: {readyPlayer.ToString()} / {PhotonNetwork.CurrentRoom.MaxPlayers.ToString()}";
    }

    [PunRPC]
    void UpdateConfirm(int _confirmPlayer, bool _isCheck)
    {
        confirmPlayer = _confirmPlayer;
        if (_isCheck)
        {
            confirmPlayer++;
            if(confirmPlayer == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                confirmPlayer = 0;
            }
        }

        joinMemberText.text = $"참가 인원: {PhotonNetwork.CurrentRoom.PlayerCount.ToString()}";
        readyPlayerText.text = $"준비: {readyPlayer.ToString()} / {PhotonNetwork.CurrentRoom.MaxPlayers.ToString()}";
    }
}
