using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TempRoom_ : MonoBehaviourPun
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

    [SerializeField] bool inRoom = true;
    [SerializeField] int m_readyPlayer = 0;
    [SerializeField] int m_confirmPlayer = 0;

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start() => photonView.RPC("UpdatePlayer", RpcTarget.MasterClient, inRoom);

    public void OnClickBack()
    {
        inRoom = false;
        photonView.RPC("UpdatePlayer", RpcTarget.MasterClient, inRoom);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TempLobby");
    }

    public void OnClickReady()
    {
        readyButton.interactable = false;
        photonView.RPC("UpdateReady", RpcTarget.MasterClient, m_readyPlayer, readyButton.interactable);
    }

    public void OnClickConfirm()
    {
        readyButton.interactable = false;
        photonView.RPC("UpdateConfirm", RpcTarget.MasterClient);
    }

    [PunRPC]
    void UpdatePlayer(bool _inRoom)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            print("ReadyPlayer and Data Update");
            if (_inRoom)
            {
                photonView.RPC("UpdateReady", RpcTarget.All, m_readyPlayer, readyButton.interactable);
            }
            else
            {
                photonView.RPC("UpdateReady", RpcTarget.All);
            }
        }

        print("Player Data Update");
        joinMemberText.text = "참가 인원: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        m_readyPlayerText.text = "준비: " + m_readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    [PunRPC]
    void UpdateReady(int _readyPlayer, bool _isReady)
    {
        if (PhotonNetwork.IsMasterClient)    // 마스터에서 업데이트
        {
            print("MasterClient - Data Synchronize");
            m_readyPlayer = _readyPlayer;   // 동기화

            if (_isReady == false)          // 버튼을 눌렀으면
            {
                m_readyPlayer++;            // 변수 변동
            }
        }
        else
        {
            return;
        }

        print("준비 업데이트");
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
    void UpdateReady()
    {
        if (PhotonNetwork.IsMasterClient)   // 마스터에서 업데이트
        {
            print("MasterClient - Data Synchronize");
            m_readyPlayer--;                      // 변수 변동
        }
    }

    [PunRPC]
    void UpdateConfirm()
    {
        m_confirmPlayer++;
        m_confirmPlayerText.text = "완료: " + m_confirmPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }
}
