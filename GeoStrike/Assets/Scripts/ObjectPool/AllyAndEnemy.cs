using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(201)]
public class AllyAndEnemy : MonoBehaviourPun
{
    //public Pool[] poolArr = new Pool[6];

    //public static readonly int ALLY = 0;
    //public static readonly int ENEMY = 1;

    //private void Awake()
    //{
    //    poolArr = GetComponentsInChildren<Pool>();




    //    if (photonView.IsMine)
    //    {
    //        this.transform.SetParent(ObjectPool.instance.transform);
    //        ObjectPool.instance.allyAndEnemyArr[ALLY] = this;

    //        //Ally Pool
    //        for (int idx = 0; idx < poolArr.Length; idx++)
    //        {
    //            ObjectPool.instance.allyAndEnemyArr[ALLY].poolArr[idx].InitObjectPool(ObjectPool.instance.unitCount);
    //        }
    //    }
    //    else
    //    {
    //        this.transform.SetParent(ObjectPool.instance.transform);
    //        ObjectPool.instance.allyAndEnemyArr[ENEMY] = this;

    //        //Enemy Pool
    //        for (int idx = 0; idx < poolArr.Length; idx++)
    //        {
    //            ObjectPool.instance.allyAndEnemyArr[ENEMY].poolArr[idx].InitObjectPool(ObjectPool.instance.unitCount);
    //        }
    //    }
    //}
}
