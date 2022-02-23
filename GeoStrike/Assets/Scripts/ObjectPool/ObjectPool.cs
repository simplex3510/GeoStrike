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
    
    //public AllyAndEnemy[] allyAndEnemyArr = new AllyAndEnemy[2];
   

    private void Awake()
    {
        instance = this;

        poolArr = GetComponentsInChildren<Pool>();

        for (int idx = 0; idx < 6; idx++)
        {
            poolArr[idx].InitObjectPool(ObjectPool.instance.unitCount);
        }
    }
}
