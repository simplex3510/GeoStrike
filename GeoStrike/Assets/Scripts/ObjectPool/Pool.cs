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


    // �ʱ� Object ����
    public void InitObjectPool(int _num)
    {
        ObjPoolQueue = new Queue<Unit>();

        for (int idx = 0; idx < _num; idx++)
        {
            // Unit Create, Enqueue and SetActive false
            CreateNewObject();
        }
    }

    // Pool�� NewObject ����   
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
        // Pool�� Object�� ���� ��� - ������
        if (ObjPoolQueue.Count > 0)
        {
            Unit obj = ObjPoolQueue.Dequeue();
            obj.SetUnitActive(true);

            return obj;
        }
        // Pool�� Object�� ���� �� ��� - ���� ����� ������
        else
        {
            Unit newObj = CreateNewObject();
            ObjPoolQueue.Dequeue();
            newObj.SetUnitActive(true);

            return newObj;
        }
    }
}
