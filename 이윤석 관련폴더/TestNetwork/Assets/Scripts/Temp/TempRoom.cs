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
}

public class TempRoom : MonoBehaviourPun
{
    #region UI Field
    public Text joinMemberText;

    public Text m_readyPlayerText;
    public Button readyButton;
    
    public Text m_confirmPlayerText;
    public Text health;
    public Button confirmButton;
    public Button attackButton;
    public Button defenseButton;
    #endregion 

    [SerializeField] int  m_readyPlayer = 0;
    [SerializeField] int  m_confirmPlayer = 0;


    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start() => photonView.RPC("UpdateRoom", RpcTarget.MasterClient, EPlayerState.ENTER);

    public void OnClickBack()
    {
        readyButton.interactable = true;
        photonView.RPC("UpdateRoom", RpcTarget.MasterClient, EPlayerState.LEAVE);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TempLobby");
    }

    public void OnClickReady()
    {
        readyButton.interactable = false;
        photonView.RPC("UpdateRoom", RpcTarget.MasterClient, EPlayerState.READY);
    }

    public void OnClickConfirm()
    {
        readyButton.interactable = false;
        photonView.RPC("UpdateConfirm", RpcTarget.All);
    }

    [PunRPC]
    void UpdateRoom(EPlayerState ePlayerState)
    {
        if (PhotonNetwork.IsMasterClient && ePlayerState == EPlayerState.ENTER)
        {
            photonView.RPC("UpdatePlayer", RpcTarget.All, m_readyPlayer, false);
        }
        else if (PhotonNetwork.IsMasterClient && ePlayerState == EPlayerState.READY)
        {
            photonView.RPC("UpdatePlayer", RpcTarget.All, m_readyPlayer, true);
        }
        else if (PhotonNetwork.IsMasterClient && ePlayerState == EPlayerState.LEAVE)
        {
            photonView.RPC("UpdatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    void UpdatePlayer(int _readyPlayer, bool _isReady)
    {
        print("준비 인원 업데이트");
        m_readyPlayer = _readyPlayer;
        if (_isReady)
        {
            m_readyPlayer++;
        }

        print("인원 정보 업데이트");
        joinMemberText.text = "참가 인원: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        m_readyPlayerText.text = "준비: " + m_readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        if (m_readyPlayer == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            m_readyPlayerText.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(false);

            m_confirmPlayerText.gameObject.SetActive(true);
            health.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(true);
            attackButton.gameObject.SetActive(true);
            defenseButton.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    void UpdatePlayer()
    {
        print("준비 감소 업데이트");
        m_readyPlayer--;

        print("인원 정보 업데이트");
        joinMemberText.text = "참가 인원: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        m_readyPlayerText.text = "준비: " + m_readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    [PunRPC]
    void UpdateConfirm()
    {
        m_confirmPlayer++;
        m_confirmPlayerText.text = "완료: " + m_confirmPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }
}
