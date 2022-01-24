using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Battle : MonoBehaviourPun
{
    int confirm = 0;
    int health = 100;

    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        
    }
}
