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
        
    }

    private void OnEnable()
    {
        unitPool = new Queue<Unit>();
    }

    private void Start()
    {
        unitPool.Enqueue(CreateUnit());
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

        InitalizeUnit(newUnit);

        return newUnit;
    }

    [PunRPC]
    void InitalizeUnit(Unit newUnit)
    {
        newUnit.myPool = unitPool;
        newUnit.myParent = transform;

        if (newUnit.photonView.IsMine)
        {
            newUnit.transform.SetParent(newUnit.myParent);
            newUnit.photonView.RPC("InitalizeUnit", RpcTarget.Others, newUnit);
        }
    }
}