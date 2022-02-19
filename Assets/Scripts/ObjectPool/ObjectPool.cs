using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPool : MonoBehaviourPun
{
    public static ObjectPool instance;
    public readonly int MINE = 0;
    public readonly int YOURS = 1;
    public readonly int UNIY_KIND = 6;

    public AllyAndEnemy[] allyAndEnemyArr = new AllyAndEnemy[2];

    public int unitCount;

    private void Awake()
    {
        instance = this;

        if (allyAndEnemyArr == null) { allyAndEnemyArr = GetComponentsInChildren<AllyAndEnemy>(); }
    
        if (photonView.IsMine)
        {
            for (int idx = 0; idx < UNIY_KIND; idx++) 
            {
                allyAndEnemyArr[MINE].poolArr[idx].InitObjectPool(unitCount);
            }
        }
        else
        {
            for (int idx = 0; idx < allyAndEnemyArr.Length; idx++)
            {
                allyAndEnemyArr[YOURS].poolArr[idx].InitObjectPool(unitCount);
            }
        }
    }
}
