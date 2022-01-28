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
    // RpcTarget.All로 수신
    void Confirm() 
    {
        photonView.RPC("Battle", RpcTarget.All);    // instantiate로 하면 all, 아니면 others
    }

    [PunRPC]
    void Battle(int _damage, bool _isDefense, bool _isPrepareAttack)
    {
        if(isDefense)
        {
            damage = 0;	
        }

        health -= damage;
        healthText.text = health.ToString();
        photonView.RPC("UpdateResult", RpcTarget.All);

        // Post-Processing
        damage = 0;
        isDefense = false;
    }

    public void OnClickAttackOrPrepareAttack()
    {
        if (isPrepareAttack)
        {
            damage = Random.Range(MIN_DAMAGE, MAX_DAMAGE + 1);
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
