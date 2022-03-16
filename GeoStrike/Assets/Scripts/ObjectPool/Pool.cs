using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pool : MonoBehaviourPun
{
    public Queue<Unit> ObjPoolQueue;

    public Unit unitP1;
    public Unit unitP2;

    //public GameObject testBlue;


    // 초기 Object 생성
    public void InitObjectPool(int _num)
    {
        ObjPoolQueue = new Queue<Unit>();

        for (int idx = 0; idx < _num; idx++)
        {
            // Unit Create, Enqueue and SetActive false
            CreateNewObject();
        }
    }

    // Pool에 NewObject 생성   
    public Unit CreateNewObject()
    {
        Unit newObj;

        if (PhotonNetwork.IsMasterClient)
        {
          
           newObj = PhotonNetwork.Instantiate("Units/BlueTeam/" + unitP1.name, transform.position, Quaternion.Euler(0f, 90f, 0f)).GetComponent<Unit>();
        }
        else
        {
          
            newObj = PhotonNetwork.Instantiate("Units/RedTeam/" + unitP2.name, transform.position, Quaternion.Euler(0f, -90f, 0f)).GetComponent<Unit>();
        }

        newObj.myPool = ObjPoolQueue;
        newObj.transform.SetParent(this.transform);
        newObj.SetUnitActive(false);

        return newObj;
    }

    public Unit GetObject()
    {
        // Pool에 Object가 있을 경우 - 꺼내기
        if (ObjPoolQueue.Count > 0)
        {
            Unit obj = ObjPoolQueue.Dequeue();
            obj.SetUnitActive(true);

            return obj;
        }
        // Pool에 Object가 부족 할 경우 - 새로 만들고 꺼내기
        else
        {
            Unit newObj = CreateNewObject();
            ObjPoolQueue.Dequeue();
            newObj.SetUnitActive(true);

            return newObj;
        }
    }
}
