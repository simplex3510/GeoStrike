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

    [SerializeField] int confirm = 0;
    [SerializeField] int health = 100;
    [SerializeField] int damage = 0;
    [SerializeField] bool isCheck = false;
    [SerializeField] bool isPrepareAttack = false;
    [SerializeField] bool isDefense = false;
    EResult eResult = EResult.BATTLE;

    void Start()
    {

    }

    public void OnClickConfirm()
    {
        confirmButton.interactable = false;
        photonView.RPC("UpdateGame", RpcTarget.All);
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
        print($"Confirm - {this.gameObject.GetComponent<PhotonView>().Owner.NickName}");

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
        print($"Confirm - {this.gameObject.GetComponent<PhotonView>().Owner.NickName}");

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
}
