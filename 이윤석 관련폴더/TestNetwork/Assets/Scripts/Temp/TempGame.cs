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
    public Text healthText;

    readonly int MIN_DAMAGE = 10;
    readonly int MAX_DAMAGE = 25;

    int confirm = 0;
    int health = 100;
    int damage = 0;
    bool isPrepareAttack = false;
    bool isDefense = false;
    EResult eResult = EResult.BATTLE;

    [PunRPC]
    void Confirm()
    {
        damage = 0;
        isDefense = false;
        photonView.RPC("Battle", RpcTarget.Others);
    }

    [PunRPC]
    void Battle()
    {
        if(isDefense)
        {
            damage = 0;	
        }

        health -= damage;
        healthText.text = health.ToString();
        photonView.RPC("ApplyResult", RpcTarget.All);
    }

    public void OnClickAttackOrPrepareAttack()
    {
        if(isPrepareAttack == false)
        {
            isPrepareAttack = true;
        }
        else
        {
            // photonView.RPC("OnClickAttackOrPrepareAttack")
            damage = Random.Range(MIN_DAMAGE, MAX_DAMAGE + 1);
        }
    }

    public void OnClickDefense()
    {
        isDefense = true;
    }

    [PunRPC]
    void ApplyResult()
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
