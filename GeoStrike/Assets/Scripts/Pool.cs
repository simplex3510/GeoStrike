using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public Queue<Unit> poolingObjectQueue = new Queue<Unit>();

    [SerializeField] private int unitNum;
    public Unit unit;

    private void Awake()
    {
        InitObjectPool(unitNum);
    }

    // �ʱ� Object ����
    private void InitObjectPool(int _num)
    {
        for (int idx = 0; idx < _num; idx++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    // Pool�� NewObject ����
    private Unit CreateNewObject()
    {
        Unit obj = Instantiate(unit, transform);
        obj.gameObject.SetActive(false);
        return obj;
    }

    // Pool���� ���� ��������
    public Unit GetObject()
    {
        // Pool�� Object�� ���� ���
        if (poolingObjectQueue.Count > 0)
        {
            Unit obj = poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        // Pool�� Object�� ���� �� ���
        else
        {
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
