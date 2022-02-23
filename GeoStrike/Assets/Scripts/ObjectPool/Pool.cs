using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pool : MonoBehaviour
{
    public Queue<Unit> poolingObjectQueue = new Queue<Unit>();

    public Unit unit;

    // �ʱ� Object ����
    public void InitObjectPool(int _num)
    {
        for (int idx = 0; idx < _num; idx++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    // Pool�� NewObject ����
    private Unit CreateNewObject()
    {
        Unit _unit = PhotonNetwork.Instantiate(unit.name, transform.position, Quaternion.identity).GetComponent<Unit>();
        return _unit;
    }

    // Pool���� ���� ��������
    public Unit GetObject()
    {
        // Pool�� Object�� ���� ���
        if (poolingObjectQueue.Count > 0)
        {
            Debug.Log("get Object");
            Unit obj = poolingObjectQueue.Dequeue();
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

    // ���� Object�� Pool�� ��ȯ
    private void ReturnObject(Unit _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(_obj);
    }

}
