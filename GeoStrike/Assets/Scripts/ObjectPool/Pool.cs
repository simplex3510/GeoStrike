using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pool : MonoBehaviourPun
{
    public Queue<Unit> p1ObjPoolQueue = new Queue<Unit>();
    public Queue<Unit> p2ObjPoolQueue = new Queue<Unit>();

    public Unit unit;

    // 초기 Object 생성
    public void InitObjectPool(int _num)
    {
        for (int idx = 0; idx < _num; idx++)
        {
            CreateNewObject();
        }
    }

    // Pool에 NewObject 생성   
    public Unit CreateNewObject()
    {
        Unit newObj = PhotonNetwork.Instantiate(unit.name, transform.position, Quaternion.identity).GetComponent<Unit>();
        return newObj;
    }

    [PunRPC]
    public Unit GetObject()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("GetObject", RpcTarget.Others);
        }

        if(photonView.IsMine && PhotonNetwork.IsMasterClient || !photonView.IsMine && !PhotonNetwork.IsMasterClient)
        {
            if (p1ObjPoolQueue.Count > 0)
            {
                Unit obj = p1ObjPoolQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);

                return obj;
            }
            // Pool에 Object가 부족 할 경우 - P1
            else
            {
                Debug.Log("p1-New");
                Unit newObj = CreateNewObject();
                Debug.Log(p1ObjPoolQueue.Count);
                p1ObjPoolQueue.Dequeue();
                newObj.transform.SetParent(null);
                newObj.gameObject.SetActive(true);

                return newObj;
            }
        }
        else
        {
            if (p2ObjPoolQueue.Count > 0)
            {
                Unit obj = p2ObjPoolQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);

                return obj;
            }
            // Pool에 Object가 부족 할 경우 - P1
            else
            {
                Debug.Log("p2-New");
                Unit newObj = CreateNewObject();
                Debug.Log(p2ObjPoolQueue.Count);
                p2ObjPoolQueue.Dequeue();
                newObj.transform.SetParent(null);
                newObj.gameObject.SetActive(true);

                return newObj;
            }
        }
    }

    // 사용된 Object를 Pool로 반환
    private void ReturnObject(Unit _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(transform);
        p1ObjPoolQueue.Enqueue(_obj);
    }

}
