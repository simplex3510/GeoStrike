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
    public Text nicknameText;
    public Text localHealthText;
    public Text remoteHealthText;
    public Button confirmButton;
    public Button attackButton;
    public Button defenseButton;
    #endregion

    readonly int MIN_DAMAGE = 10;
    readonly int MAX_DAMAGE = 25;
    readonly int MAX_PLAYER = 2;

    [SerializeField] int confirm = 0;
    [SerializeField] int health = 100;
    [SerializeField] int damage = 0;
    GameObject gameUI;
    bool isPrepareAttack = false;
    bool isDefense = false;
    EResult eResult = EResult.BATTLE;

    void OnEnable()
    {
        gameUI = PhotonNetwork.Instantiate("gameUIPrefab", Vector3.zero, Quaternion.identity);
        photonView.RPC("UpdateGameUI", RpcTarget.All);
    }

    public void OnClickConfirm()
    {
        // gameUI[0].transform.GetChild(3).GetComponent<Button>().interactable = false;
        photonView.RPC("UpdateConfirm", RpcTarget.MasterClient);
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
    void UpdateConfirm(int _confirmPlayer, bool _isCheck)
    {
        print($"Confirm - {photonView.Owner}");

        confirm = _confirmPlayer;
        if (_isCheck)
        {
            confirm++;
            if (confirm == MAX_PLAYER)
            {
                confirm = 0;
                photonView.RPC("Battle", RpcTarget.Others, damage, isDefense, isPrepareAttack);
                //gameUI[0].transform.GetChild(3).GetComponent<Button>().interactable = true;
                //gameUI[1].transform.GetChild(3).GetComponent<Button>().interactable = true;
            }
        }

        //gameUI[0].transform.GetChild(1).GetComponent<Text>().text = $"완료: {confirmPlayer.ToString()} / {MAX_PLAYER.ToString()}";
        //gameUI[1].transform.GetChild(1).GetComponent<Text>().text = $"완료: {confirmPlayer.ToString()} / {MAX_PLAYER.ToString()}";
    }

    [PunRPC]
    public void Battle(int _damage, bool _isDefense, bool _isPrepareAttack)
    {
        print($"Battle - {photonView.Owner}");
        if (isDefense)
        {
            damage = 0;
        }

        health -= damage;
        localHealthText.text = health.ToString();
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
        var gameUI = GameObject.FindGameObjectsWithTag("GameUI");

        for (int i = 0; i < gameUI.Length; i++)
        {
            gameUI[i].transform.SetParent(gridPanel.transform);

            gameUI[i].transform.GetChild(0).GetComponent<Text>().text = $"[{gameUI[i].GetComponent<PhotonView>().Owner.NickName}]";             // 플레이어 닉네임
            gameUI[i].transform.GetChild(1).GetComponent<Text>().text = $"완료: {confirm.ToString()} / {MAX_PLAYER.ToString()}";
            gameUI[i].transform.GetChild(2).GetComponent<Text>().text = $"HP: {health.ToString()}";
            if (gameUI[i].GetComponent<PhotonView>().Owner.IsLocal)
            {
                localHealthText = gameUI[i].transform.GetChild(2).GetComponent<Text>();
            }
            else
            {
                remoteHealthText = gameUI[i].transform.GetChild(2).GetComponent<Text>();
            }

            if (photonView.IsMine)
            {
                gameUI[i].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(OnClickConfirm);                    // 
                confirmButton = gameUI[0].transform.GetChild(3).GetComponent<Button>();
                gameUI[i].transform.GetChild(4).GetComponent<Button>().onClick.AddListener(OnClickAttackOrPrepareAttack);      //
                attackButton = gameUI[0].transform.GetChild(4).GetComponent<Button>();
                gameUI[i].transform.GetChild(5).GetComponent<Button>().onClick.AddListener(OnClickDefense);                    // 
                defenseButton = gameUI[0].transform.GetChild(5).GetComponent<Button>();

                gameUI[i].transform.GetChild(6).gameObject.SetActive(false);   // 패널 비활성화 - 클릭 차단 해제
            }
        }
    }
}
