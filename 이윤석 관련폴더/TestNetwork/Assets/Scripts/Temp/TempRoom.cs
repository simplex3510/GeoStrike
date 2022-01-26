using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

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
    [SerializeField] bool m_inRoom = true;


    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start() => photonView.RPC("UpdatePlayer", RpcTarget.MasterClient);

    public void OnClickBack()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TempLobby");
    }

    public void OnClickReady()
    {
        readyButton.interactable = false;
        photonView.RPC("UpdateReady", RpcTarget.MasterClient, m_readyPlayer);
 
    }

    public void OnClickConfirm()
    {
        readyButton.interactable = false;
        photonView.RPC("UpdateConfirm", RpcTarget.All);
    }

    [PunRPC]
    void UpdatePlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateReady", RpcTarget.Others, m_readyPlayer);
        }
        print("참가 인원 업데이트");
        joinMemberText.text = "참가 인원: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        m_readyPlayerText.text = "준비: " + m_readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    [PunRPC]
    void UpdateReady(int _readyPlayer)
    {
        print("준비 업데이트");
        if(readyButton.interactable == false)
        {
            m_readyPlayer++;
        }
        else
        {
            m_readyPlayer = _readyPlayer;
        }

        m_readyPlayerText.text = "준비: " + m_readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        if(m_readyPlayer == PhotonNetwork.CurrentRoom.MaxPlayers)
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

    void UpdateReady()
    {
        print("준비 업데이트");
        if (readyButton.interactable == false)
        {
            m_readyPlayer++;
        }
        else
        {
            m_readyPlayer = _readyPlayer;
        }

        m_readyPlayerText.text = "준비: " + m_readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    [PunRPC]
    void UpdateConfirm()
    {
        m_confirmPlayer++;
        m_confirmPlayerText.text = "완료: " + m_confirmPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }
}
