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
        if (photonView.IsMine)
        {
            for (int idx = 0; idx < 6; idx++)
            {
                if (unitIdx - 1 == idx)
                {
                    this.transform.SetParent(ObjectPool.instance.poolArr[idx].transform);
                    break;
                }
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

}



