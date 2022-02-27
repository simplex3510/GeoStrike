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

    // �ʱ� Object ����
    public void InitObjectPool(int _num)
    {
        for (int idx = 0; idx < _num; idx++)
        {
            CreateNewObject();
        }
    }

    // Pool�� NewObject ����   
    public Unit CreateNewObject()
    {
        Unit newObj = PhotonNetwork.Instantiate(unit.name, transform.position, Quaternion.identity).GetComponent<Unit>();
        return newObj;
    }

    [PunRPC]
    public Unit GetObject()
    {
        // if(p1)
        // Pool�� Object�� ���� ��� - P1
        if (p1ObjPoolQueue.Count > 0)
        {
            Unit obj = p1ObjPoolQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);

            return obj;
        }
        // Pool�� Object�� ���� �� ��� - P1
        else
        {
            Unit newObj = CreateNewObject();
            p1ObjPoolQueue.Dequeue();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            
            return newObj;
        }
        // else (p2)
        if (p2ObjPoolQueue.Count > 0)
        {
            Unit obj = p2ObjPoolQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);

            return obj;
        }
        // Pool�� Object�� ���� �� ��� - P1
        else
        {
            Unit newObj = CreateNewObject();
            p2ObjPoolQueue.Dequeue();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);

            return newObj;
        }
    }

    // ���� Object�� Pool�� ��ȯ
    private void ReturnObject(Unit _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(transform);
        p1ObjPoolQueue.Enqueue(_obj);
    }

}
