using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Unit : MonoBehaviourPun
{
    public UnitInfo unitInfo;
    public int unitIdx;

    public UnitMovement unitMovement;


    private void Awake()
    {
        this.gameObject.SetActive(false);
        this.transform.SetParent(GameObject.Find("P1Pool_Unit" + unitInfo.unitName).transform);
    }
}



