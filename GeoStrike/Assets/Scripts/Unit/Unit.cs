using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Photon.Pun;
using Photon.Realtime;

public class Unit : MonoBehaviourPun
{
    public Transform myParent;
    public Queue<Unit> myPool;

    public UnitInfo unitInfo;
    public int unitIdx;

    public UnitMovement unitMovement;

    private void Update()
    {
        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = this.photonView.ViewID.ToString();
        }
    }

    // Return to your pool and set your parent
    protected virtual void OnDisable()
    {
        if(photonView.IsMine)
        {
            transform.SetParent(myParent);
            myPool.Enqueue(this);
        }
    }

    [PunRPC]
    public void SetUnitActive(bool isTrue)
    {
        gameObject.SetActive(isTrue);
    }
}



