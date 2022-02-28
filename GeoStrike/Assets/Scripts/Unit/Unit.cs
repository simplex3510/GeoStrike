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
    public PhotonView pv;


    private void Awake()
    {
        this.pv = GetComponent<PhotonView>();
        this.gameObject.SetActive(false);
        this.transform.SetParent(GameObject.Find("Pool_Unit" + unitInfo.unitName).transform);

        if (photonView.IsMine && PhotonNetwork.IsMasterClient || !photonView.IsMine && !PhotonNetwork.IsMasterClient)
        {
            ObjectPoolMgr.instance.poolArr[unitIdx - 1].p1ObjPoolQueue.Enqueue(this);
            Debug.Log("EnQ : " + ObjectPoolMgr.instance.poolArr[unitIdx - 1].p1ObjPoolQueue.Count);
        }
        else
        {
            ObjectPoolMgr.instance.poolArr[unitIdx - 1].p2ObjPoolQueue.Enqueue(this);
        }
    }

    private void Update()
    {
        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = this.pv.ViewID.ToString();
        }
    }
}



