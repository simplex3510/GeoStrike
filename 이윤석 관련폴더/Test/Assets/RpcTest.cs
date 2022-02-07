using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RpcTest : MonoBehaviour
{
    PhotonView pv;
    public Text text;
    public Text IsM;
    public Button bt;
    public static int count;

   public void SendTest()
    {
        pv.RPC("CountUp", RpcTarget.MasterClient);
    }

    [PunRPC]
    void CountUp()
    {
        count++;
        pv.RPC("UPUP", RpcTarget.All, count);
        pv.RPC("UPUPOther", RpcTarget.All, count);
    }

    [PunRPC]
    void UPUP(int c )
    {
        count = c;
        text.text = count.ToString();       
    }
    [PunRPC]
    void UPUPOther(int c)
    {
        for (int i = 0; i < testjoinOk.pvList.Count; i++)
        {
            if (testjoinOk.pvList[i] != this)
            {
                testjoinOk.pvList[i].text.text = count.ToString(); 
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        
        this.transform.SetParent(GameObject.Find("Canvas").transform);

        if (pv.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                IsM.text = "마스터";
               
}
            else
            {
                IsM.text = "게스트";
              
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                IsM.text = "게스트";
               
            }
            else
            {
                IsM.text = "마스터";
               
            }
        }
        if (pv.IsMine)
        {
        bt.interactable = true;
        }
        else
        {
            bt.interactable = false;
        }

        testjoinOk.pvList.Add(this);
    }

    private void Update()
    {
        
    }
}
