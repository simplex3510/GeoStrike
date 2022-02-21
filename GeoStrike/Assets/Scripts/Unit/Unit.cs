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

    public Transform allyAndEnemyParent;
    public Transform poolParent;

    private void Start()
    {
        if (photonView.IsMine)
        {
            allyAndEnemyParent = ObjectPool.instance.allyAndEnemyArr[AllyAndEnemy.ALLY].transform; //GameObject.Find("Pool_Unit" + gameObject.name[0]).transform;

            transform.SetParent(allyAndEnemyParent);
            gameObject.SetActive(false);
        }
        else
        {
            allyAndEnemyParent = GameObject.Find("Pool_Unit" + gameObject.name[0]).transform;

            transform.SetParent(allyAndEnemyParent);
            gameObject.SetActive(false);
        }

        
    }

}



