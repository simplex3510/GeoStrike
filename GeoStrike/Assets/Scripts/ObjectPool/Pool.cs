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
            newObj = PhotonNetwork.Instantiate(unitP1.name, Vector3.zero, Quaternion.identity).GetComponent<Unit>();
        }
        else
        {
            newObj = PhotonNetwork.Instantiate(unitP2.name, Vector3.zero, Quaternion.Euler(0f, 0f, 180f)).GetComponent<Unit>();
        }

        // newObj.transform.SetParent(GameObject.Find("Pool_Unit" + unit.unitInfo.unitName).transform);

        newObj.myPool = ObjPoolQueue;
        newObj.myParent = transform;

        newObj.transform.SetParent(newObj.myParent);
        newObj.SetUnitActive(false);

        // ObjPoolQueue.Enqueue(newObj); -> Unit�� OnDisable���� �ڵ����� Enqueue
        Debug.Log("IsMine : " + this.photonView.IsMine + " : " + newObj + " : " + ObjPoolQueue.Count);
        return newObj;
    }

    public Unit GetObject()
    {
        // Pool�� Object�� ���� ��� - ������
        if (ObjPoolQueue.Count > 0)
        {
            Unit obj = ObjPoolQueue.Dequeue();
            Debug.Log("Get Obj ID : " + obj.photonView.ViewID);
            obj.transform.SetParent(null);
            obj.SetUnitActive(true);

            return obj;
        }
        // Pool�� Object�� ���� �� ��� - ���� ����� ������
        else
        {
            Unit newObj = CreateNewObject();
            Debug.Log("Create New Obj ID : " + newObj.photonView.ViewID);

            ObjPoolQueue.Dequeue();
            newObj.transform.SetParent(null);
            newObj.SetUnitActive(true);

            return newObj;
        }
    }

    // ���� Object�� Pool�� ��ȯ
    //private void ReturnObject(Unit _obj)
    //{
    //    _obj.gameObject.SetActive(false);
    //    _obj.transform.SetParent(transform);
    //    p1ObjPoolQueue.Enqueue(_obj);
    //}
}
