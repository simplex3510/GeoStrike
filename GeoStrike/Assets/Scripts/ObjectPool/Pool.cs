using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pool : MonoBehaviourPun
{
    public Queue<Unit> ObjPoolQueue = new Queue<Unit>();

    public Unit unit;

    // �ʱ� Object ����
    public void InitObjectPool(int _num)
    {
        for (int idx = 0; idx < _num; idx++)
        {
            CreateNewObject();
        }
    }

    [PunRPC]
    // Pool�� NewObject ����   
    public Unit CreateNewObject()
    {
        Unit newObj = PhotonNetwork.Instantiate(unit.name, transform.position, Quaternion.identity).GetComponent<Unit>();
        
        //if (photonView.IsMine)
        //{
        //    photonView.RPC("CreateNewObject", RpcTarget.Others);
        //    Debug.Log("CreateNewObject RPC");
        //}

        newObj.transform.SetParent(GameObject.Find("Pool_Unit" + unit.unitInfo.unitName).transform);
        //SetUnitActive(newObj, false);
        ObjPoolQueue.Enqueue(newObj);
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
