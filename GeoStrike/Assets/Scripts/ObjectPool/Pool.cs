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
        if (photonView.IsMine)
        {
            for (int idx = 0; idx < _num; idx++)
            {
                p1ObjPoolQueue.Enqueue(CreateNewObject());
            }
        }
        else
        {
            for (int idx = 0; idx < _num; idx++)
            {
                p2ObjPoolQueue.Enqueue(CreateNewObject());
            }
        }
    }

    // Pool�� NewObject ����
    private Unit CreateNewObject()
    {
        Unit newObj = PhotonNetwork.Instantiate(unit.name, transform.position, Quaternion.identity).GetComponent<Unit>();
        return newObj;
    }

    // Pool���� ���� ��������
    public Unit GetObject()
    {
        if (photonView.IsMine)
        {
            // Pool�� Object�� ���� ���
            if (p1ObjPoolQueue.Count > 0)
            {
                Debug.Log("get Object");
                Unit obj = p1ObjPoolQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            // Pool�� Object�� ���� �� ���
            else
            {
                Debug.Log("creat object");
                Unit newObj = CreateNewObject();
                newObj.transform.SetParent(null);
                newObj.gameObject.SetActive(true);
                return newObj;
            }
        }
        else
        {
            // Pool�� Object�� ���� ���
            if (p2ObjPoolQueue.Count > 0)
            {
                Debug.Log("get Object");
                Unit obj = p2ObjPoolQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            // Pool�� Object�� ���� �� ���
            else
            {
                Debug.Log("creat object");
                Unit newObj = CreateNewObject();
                newObj.transform.SetParent(null);
                newObj.gameObject.SetActive(true);
                return newObj;
            }
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
