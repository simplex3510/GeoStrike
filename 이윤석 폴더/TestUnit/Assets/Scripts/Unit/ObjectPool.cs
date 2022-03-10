using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPool : MonoBehaviourPun
{
    public Queue<Unit> unitPool;
    public Unit blueUnit;
    public Unit redUnit;

    private void Awake()
    {
        unitPool = new Queue<Unit>();
    }

    private void Start()
    {
        // unitPool.Enqueue(CreateUnit()); -> Unit의 OnDisable에서 Enqueue
        print($"{unitPool.Peek()}: {unitPool.Count}");
    }

    // Instantiate is called by local computer
    Unit CreateUnit()
    {
        Unit newUnit;

        if(PhotonNetwork.IsMasterClient)
        {
            newUnit = PhotonNetwork.Instantiate(blueUnit.name, new Vector3(-8f, -1f, 0f), Quaternion.identity).GetComponent<Unit>();
        }
        else
        {
            newUnit = PhotonNetwork.Instantiate(redUnit.name, new Vector3(8f, 1f, 0f), Quaternion.Euler(0f, 0f, 180f)).GetComponent<Unit>();
        }

        newUnit.myPool = unitPool;
        newUnit.transform.SetParent(this.transform);
        newUnit.SetUnitActive(false);   // OnDisable에서 Enqueue됨

        return newUnit;
    }
}