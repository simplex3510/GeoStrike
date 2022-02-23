using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Test : MonoBehaviour
{
    public Unit[] units;

    // Start is called before the first frame update
    void Update()
    {
        units[0].GetComponent<PhotonView>().RPC("TestRPC", RpcTarget.All);
        units[1].GetComponent<PhotonView>().RPC("TestRPC", RpcTarget.All);
    }
}
