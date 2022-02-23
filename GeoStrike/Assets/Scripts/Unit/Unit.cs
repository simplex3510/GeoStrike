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

        if (photonView.IsMine)
        {
            this.transform.SetParent(GameObject.Find("Pool_Unit" + this.gameObject.name[0]).transform);

            //for (int idx = 0; idx < 6; idx++)
            //{
            //    if (unitIdx - 1 == idx)
            //    {
            //        this.transform.SetParent(ObjectPoolMgr.instance.playerPoolArr[PlayerPool.P1].poolArr[idx].transform);
            //        break;
            //    }
            //}
        }
        else
        {
            this.transform.SetParent(GameObject.Find("Pool_Unit" + this.gameObject.name[0]).transform);
            //for (int idx = 0; idx < 6; idx++)
            //{
            //    if (unitIdx - 1 == idx)
            //    {
            //        this.transform.SetParent(ObjectPoolMgr.instance.playerPoolArr[PlayerPool.P2].poolArr[idx].transform);
            //        break;
            //    }
            //}
        }
    }

}



