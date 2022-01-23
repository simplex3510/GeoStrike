using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TempRoom : MonoBehaviourPun
{
    public Text joinMemberText;

    public Text readyPlayerText;
    public Button readyButton;
    
    public Text confirmPlayerText;
    public Text health;
    public Button confirmButton;
    public Button attackButton;
    public Button defenseButton;

    [SerializeField] int readyPlayer = 0;

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Start() => photonView.RPC("UpdatePlayer", RpcTarget.All);

    [PunRPC]
    void UpdatePlayer()
    {
        print("참가 인원 업데이트");
        joinMemberText.text = "참가 인원: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        readyPlayerText.text = "준비: " + readyPlayer.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    [PunRPC]
    void UpdateReady()
    {
        print("준비 업데이트");
        readyPlayer++;
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

    public void OnClickReady()
    {
        photonView.RPC("UpdateReady", RpcTarget.All);
        readyButton.interactable = false;
    }
}
