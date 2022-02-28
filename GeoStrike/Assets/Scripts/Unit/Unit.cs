using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Unit : MonoBehaviourPun
{
    public UnitInfo unitInfo;
    public int unitIdx;

    public UnitMovement unitMovement;

    private void Awake()
    {
        SetUnitActive(false);
    }

    private void Update()
    {
        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = this.photonView.ViewID.ToString();
        }
    }

    public void SetUnitActive(bool _bool)
    {
        gameObject.SetActive(_bool);
    }
}



