using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMgr : MonoBehaviourPunCallbacks
{

    public Transform playerField;


    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Player1", new Vector3(-27, -24, 0), Quaternion.identity).transform.SetParent(playerField);
        }
        else
        {
            PhotonNetwork.Instantiate("Player2", new Vector3(37, -24, 0), Quaternion.identity).transform.SetParent(playerField);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
