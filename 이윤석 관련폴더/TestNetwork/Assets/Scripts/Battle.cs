using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NewBehaviourScript : MonoBehaviourPun
{
    int confirm = 0;
    int HP = 100;
    bool defense = false;

    [PunRPC]
    public void Battle(int Damage)
    {
        if(defense)
        {
            Damage = 0;
        }

        HP -= Damage;
        
        if(HP < 0)
        {
            // 판단
        }
    }
}
