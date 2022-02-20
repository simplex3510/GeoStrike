using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPool : MonoBehaviourPun
{
    public static ObjectPool instance;

    public int unitCount;

    public Pool[] poolArr = new Pool[6];

    private void Awake()
    {
        instance = this;

        poolArr = GetComponentsInChildren<Pool>();

        if (photonView.IsMine)
        {
            for (int idx = 0; idx < poolArr.Length; idx++) 
            {
                poolArr[idx].InitObjectPool(unitCount);
            }
        }
    }
}
