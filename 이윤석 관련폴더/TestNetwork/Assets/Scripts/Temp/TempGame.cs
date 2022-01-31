using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

enum EResult
{
    BATTLE,
    VICTORY,
    DEFEATE
}

public class TempGame : MonoBehaviourPun
{
    #region UI
    public GameObject gridPanel;

    public Text nicknameText;
    public Text confirmPlayerText;
    public Text localHealthText;
    public Button confirmButton;
    public Button attackButton;
    public Button defenseButton;
    #endregion

    readonly int MIN_DAMAGE = 10;
    readonly int MAX_DAMAGE = 25;
    readonly int MAX_PLAYER = 2;

    [SerializeField] GameObject gameUI;
    [SerializeField] int confirm = 0;
    [SerializeField] int health = 100;
    [SerializeField] int damage = 0;
    [SerializeField] bool isPrepareAttack = false;
    [SerializeField] bool isDefense = false;
    EResult eResult = EResult.BATTLE;

    void Start()
    {
        gridPanel = GameObject.Find("GridPanel");
        gameUI = this.gameObject;
        photonView.RPC("UpdateGameUI", RpcTarget.All);
    }

    public void OnClickConfirm()
    {
        confirmButton.interactable = false;
        photonView.RPC("UpdateGame", RpcTarget.MasterClient);
    }

    public void OnClickAttackOrPrepareAttack()
    {

        if (isPrepareAttack)
        {
            damage = Random.Range(MIN_DAMAGE, MAX_DAMAGE + 1);
            isPrepareAttack = false;
        }
        else
        {
            isPrepareAttack = true;
        }
    }

    public void OnClickDefense()
    {
        isDefense = true;
    }

    [PunRPC]
    void UpdateGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateConfirm", RpcTarget.All, confirm, !confirmButton.interactable);
        }
    }

    [PunRPC]
    void UpdateConfirm(int _confirm, bool _isCheck)
    {
        print($"Confirm - {gameUI.GetComponent<PhotonView>().Owner}");

        confirm = _confirm;
        if (_isCheck)
        {
            confirm++;
        }

        if (confirm == MAX_PLAYER)
        {
            confirm = 0;
            photonView.RPC("Battle", RpcTarget.Others, damage, isDefense, isPrepareAttack);
            confirmButton.interactable = true;
        }

        confirmPlayerText.text = $"완료: {confirm.ToString()} / {MAX_PLAYER.ToString()}";
    }

    [PunRPC]
    public void Battle(int _damage, bool _isDefense, bool _isPrepareAttack)
    {
        print($"Battle - {gameUI.GetComponent<PhotonView>().Owner}");

        if (isDefense)
        {
            damage = 0;
        }

        health -= damage;
        localHealthText.text = $"HP: {health.ToString()}";
        // photonView.RPC("UpdateResult", RpcTarget.All);

        // Post-Processing
        damage = 0;
        isDefense = false;
    }

    [PunRPC]
    void UpdateResult()
    {
        if (health <= 0)
        {
            print("defeat");
            PhotonNetwork.LoadLevel("TempDefeat");
        }
        else
        {
            print("victory");
            PhotonNetwork.LoadLevel("TempVictory");
        }
    }

    [PunRPC]
    void UpdateGameUI()
    {
        gameUI.transform.SetParent(gridPanel.transform);

        nicknameText = gameUI.transform.GetChild(0).GetComponent<Text>();
        nicknameText.text = $"[{gameUI.GetComponent<PhotonView>().Owner.NickName}]";

        confirmPlayerText = gameUI.transform.GetChild(1).GetComponent<Text>();
        confirmPlayerText.text = $"완료: {confirm.ToString()} / {MAX_PLAYER.ToString()}";

        localHealthText = gameUI.transform.GetChild(2).GetComponent<Text>();
        localHealthText.text = $"HP: {health.ToString()}";

        confirmButton = gameUI.transform.GetChild(3).GetComponent<Button>();
        confirmButton.onClick.AddListener(OnClickConfirm);

        attackButton = gameUI.transform.GetChild(4).GetComponent<Button>();
        attackButton.onClick.AddListener(OnClickAttackOrPrepareAttack);

        defenseButton = gameUI.transform.GetChild(5).GetComponent<Button>();
        defenseButton.onClick.AddListener(OnClickDefense);

        gameUI.transform.GetChild(6).gameObject.SetActive(false);             // 패널 비활성화 - 클릭 차단 해제
    }
}
