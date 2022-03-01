using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pool : MonoBehaviourPun
{
    public Queue<Unit> ObjPoolQueue;

    public Unit unit;

    private void Awake()
    {
        
    }

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
            newObj = PhotonNetwork.Instantiate(unit.name, new Vector3(-8f, -1f, 0f), Quaternion.identity).GetComponent<Unit>();
        }
        else
        {
            newObj = PhotonNetwork.Instantiate(unit.name, new Vector3(8f, 1f, 0f), Quaternion.Euler(0f, 0f, 180f)).GetComponent<Unit>();
        }

        // newObj.transform.SetParent(GameObject.Find("Pool_Unit" + unit.unitInfo.unitName).transform);

        newObj.myPool = ObjPoolQueue;
        newObj.myParent = transform;

        newObj.transform.SetParent(newObj.myParent);
        newObj.photonView.RPC("SetUnitActive", RpcTarget.All, false);

        // ObjPoolQueue.Enqueue(newObj); -> Unit�� OnDisable���� �ڵ����� Enqueue
        Debug.Log("IsMine : " + this.photonView.IsMine + " : " + newObj + " : " + ObjPoolQueue.Count);
        return newObj;
    }

    public Unit GetObject()
    {
        // Pool�� Object�� ���� ��� - ������
        //if (p1ObjPoolQueue.Count > 0)
        //{
        //    Unit obj = p1ObjPoolQueue.Dequeue();
        //    obj.transform.SetParent(null);
        //    obj.gameObject.SetActive(true);

        //    return obj;
        //}
        //// Pool�� Object�� ���� �� ��� - ���� ����� ������
        //else
        //{
        //    Debug.Log("p1-New");
        //    Unit newObj = CreateNewObject();
        //    SetUnitEnqueue(newObj);
        //    Debug.Log(p1ObjPoolQueue.Count);
        //    p1ObjPoolQueue.Dequeue();
        //    newObj.transform.SetParent(null);
        //    newObj.gameObject.SetActive(true);

        //    return newObj;
        //}
        return null;
    }

    // ���� Object�� Pool�� ��ȯ
    //private void ReturnObject(Unit _obj)
    //{
    //    _obj.gameObject.SetActive(false);
    //    _obj.transform.SetParent(transform);
    //    p1ObjPoolQueue.Enqueue(_obj);
    //}
}
