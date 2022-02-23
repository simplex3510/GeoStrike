using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPoolMgr : MonoBehaviourPun
{
    public static ObjectPoolMgr instance;

    public int unitCount;

    public Pool[] poolArr = new Pool[6];

    private void Awake()
    {
        instance = this;

        poolArr = GetComponentsInChildren<Pool>();

        foreach (Pool pool in poolArr)
        {
            pool.InitObjectPool(unitCount);
        }
    }
}
