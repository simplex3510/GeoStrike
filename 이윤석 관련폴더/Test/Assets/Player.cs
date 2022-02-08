using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 tempPos = Vector3.zero;

            tempPos.x = h;
            tempPos.z = v;
            this.transform.position += tempPos;
        }
    }
}
