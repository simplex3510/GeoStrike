using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[DefaultExecutionOrder(202)]
public class Giveup : MonoBehaviour
{
    public GameObject nexus;

    private void Awake()
    {
        if (GameMgr.isMaster)
        {
            nexus = GameObject.Find("Nexus_Blue(Clone)");
        }
        else
        {

        }
    }

    public void giveup()
    {
         //nexus.OnDamaged(99999);
    }
}
