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
    LEAVE
}

public class TempRoom : MonoBehaviourPun
{
    #region gameUI Field
    public RectTransform readyUIPrefab;
    public RectTransform gameUIPrefab;

    public Canvas canvas;
    public RectTransform gridPanel;
    public Text joinMemberText;
    public Text readyPlayerText;
    public Button readyButton;
    #endregion 

    readonly int MAX_PLAYER = 2;
    [SerializeField] int readyPlayer = 0;
    GameObject readyUI;

    void Awake() => Screen.SetResolution(1920, 1080, false);
    
    // 준비 gameUI 생성
    void Start()    
    {
        // 준비 gameUI 생성 및 위치 설정
        readyUI = Instantiate(readyUIPrefab, new Vector3(960, 540, 0), Quaternion.identity).gameObject;
        readyUI.transform.SetParent(canvas.transform);

        // 준비 gameUI 개체 할당
        joinMemberText = readyUI.transform.GetChild(0).GetComponent<Text>();    // 현재 멤버 텍스트 할당
        readyPlayerText = readyUI.transform.GetChild(1).GetComponent<Text>();   // 준비 멤버 텍스트 할당
        readyButton = readyUI.transform.GetChild(2).GetComponent<Button>();     // 준비 버튼 할당
        readyButton.onClick.AddListener(OnClickReady);                          // 준비 버튼에 메서드 할당

        photonView.RPC("UpdateRoom", RpcTarget.MasterClient, EPlayerState.ENTER, false);
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

    [PunRPC]
    void UpdateRoom(EPlayerState ePlayerState, bool _isCheck)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(ePlayerState == EPlayerState.ENTER)
            {
                photonView.RPC("UpdateReady", RpcTarget.All, readyPlayer, _isCheck);
            }
            else if (ePlayerState == EPlayerState.READY)
            {
                photonView.RPC("UpdateReady", RpcTarget.All, readyPlayer, _isCheck);
            }
            else if (ePlayerState == EPlayerState.LEAVE)
            {
                photonView.RPC("UpdateReady", RpcTarget.All, _isCheck);
            }
        }
    }

    [PunRPC]
    void UpdateReady(int _readyPlayer, bool _isCheck)
    {
        readyPlayer = _readyPlayer;
        if (_isCheck)
        {
            readyPlayer++;
        }

        // 준비 gameUI 업데이트
        joinMemberText.text = $"참가 인원: {PhotonNetwork.CurrentRoom.PlayerCount.ToString()}";
        readyPlayerText.text = $"준비: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";

        if (readyPlayer == MAX_PLAYER)
        {
            Destroy(readyUI);
            PhotonNetwork.Instantiate("GameUIPrefab", Vector3.zero, Quaternion.identity);
        }
    }

    [PunRPC]
    void UpdateReady(bool _isCheck)
    {
        if(_isCheck)
        {
            readyPlayer--;
        }

        joinMemberText.text = $"참가 인원: {(PhotonNetwork.CurrentRoom.PlayerCount - 1).ToString()}";
        readyPlayerText.text = $"준비: {readyPlayer.ToString()} / {MAX_PLAYER.ToString()}";
    }
}
