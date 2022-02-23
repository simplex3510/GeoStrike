using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPool : MonoBehaviourPun
{
    public static ObjectPool instance;

    public int unitCount;

    public AllyAndEnemy[] allyAndEnemyArr = new AllyAndEnemy[2];
   

    private void Awake()
    {
        instance = this;

        allyAndEnemyArr = GetComponentsInChildren<AllyAndEnemy>();
    }
}
