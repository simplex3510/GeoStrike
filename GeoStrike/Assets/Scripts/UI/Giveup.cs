using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[DefaultExecutionOrder(202)]
public class Giveup : MonoBehaviour
{
    public Nexus nexus;

    private void Awake()
    {
        if (GameMgr.isMaster)
        {
            nexus = GameObject.Find("Nexus_Blue(Clone)").GetComponent<Nexus>();
        }
        else
        {
            nexus = GameObject.Find("Nexus_Red(Clone)").GetComponent<Nexus>();
        }
    }

    public void giveup()
    {
         nexus.OnDamaged(99999);
    }
}
