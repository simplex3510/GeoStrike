using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPool : MonoBehaviourPun
{
    public Queue<Unit> unitPool;
    public Unit unit;

    private void Awake()
    {
        unitPool = new Queue<Unit>();
    }

    // Instantiate is called by local computer
    Unit CreateUnit()
    {
        Unit newUnit;

        newUnit = PhotonNetwork.Instantiate(unit.name, new Vector3(-8f, -1f, 0f), Quaternion.identity).GetComponent<Unit>();
        
        newUnit.myPool = unitPool;
        newUnit.myParent = transform;

        // unitPool.Enqueue(newUnit);

        return newUnit;
    }

    /*
     * 
     */
}
