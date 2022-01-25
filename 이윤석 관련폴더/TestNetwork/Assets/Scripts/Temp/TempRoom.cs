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

    public Text readyPlayerText;
    public Button readyButton;
    
    public Text confirmPlayerText;
    public Text health;
    public Button confirmButton;
    public Button attackButton;
    public Button defenseButton;
    #endregion 

    [SerializeField] int readyPlayer = 0;
    [SerializeField] int confirmPlayer = 0;
    //public Text temp;

    //private void Update()
    //{
    //    temp.text = PhotonNetwork.IsMasterClient.ToString();
    //}

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
        photonView.RPC("UpdateReady", RpcTarget.All, readyPlayer);
 
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
            photonView.RPC("UpdateReady", RpcTarget.Others, readyPlayer);
        }
        print("참가 인원 업데이트");
        joinMemberText.text = "참가 인원: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        readyPlayerText.text = "준비: " + readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    [PunRPC]
    void UpdateReady(int newReadyPlayer)
    {
        print("준비 업데이트");
        if(readyButton.interactable == false)
        {
            readyPlayer++;
        }
        else
        {
            readyPlayer = newReadyPlayer;
        }

        readyPlayerText.text = "준비: " + readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        if(readyPlayer == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            readyPlayerText.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(false);

            confirmPlayerText.gameObject.SetActive(true);
            health.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(true);
            attackButton.gameObject.SetActive(true);
            defenseButton.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    void UpdateConfirm()
    {
        confirmPlayer++;
        confirmPlayerText.text = "완료: " + confirmPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }
}
