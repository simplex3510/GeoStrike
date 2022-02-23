using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(201)]
public class PlayerPool : MonoBehaviourPun
{
    public Pool[] poolArr = new Pool[6];

    public static readonly int P1 = 0;
    public static readonly int P2 = 1;

    private void Awake()
    {
        poolArr = GetComponentsInChildren<Pool>();

        this.transform.SetParent(ObjectPoolMgr.instance.transform);

        if (this.gameObject.name == "P1Pool(Clone)")
        {
            ObjectPoolMgr.instance.playerPoolArr[P1] = this;

            for (int idx = 0; idx < poolArr.Length; idx++)
            {
                ObjectPoolMgr.instance.playerPoolArr[P1].poolArr[idx].InitObjectPool(ObjectPoolMgr.instance.unitCountx2);
            }
        }

        if ( this.gameObject.name == "P2Pool(Clone)")
        {
            ObjectPoolMgr.instance.playerPoolArr[P2] = this;

            for (int idx = 0; idx < poolArr.Length; idx++)
            {
                ObjectPoolMgr.instance.playerPoolArr[P2].poolArr[idx].InitObjectPool(ObjectPoolMgr.instance.unitCountx2);
            }
        }
    }
}
