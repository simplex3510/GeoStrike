using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr : MonoBehaviour
{
    public static ObjectPoolMgr instance;

    public Pool[] poolArr;

    public int unitIdx;
    public Unit unit;

    private void Awake()
    {
        instance = this;

        poolArr = GetComponentsInChildren<Pool>();
        
        InitObjectPool(20);
    }

    // �ʱ� Object ����
    private void InitObjectPool(int _num)
    {
        for (int arrIdx = 0; arrIdx < poolArr.Length; arrIdx++)
        {
            for (int idx = 0; idx < _num; idx++)
            {
                poolArr[arrIdx].poolingObjectQueue.Enqueue(CreateNewObject(poolArr[arrIdx].unitIdx));
            }
        }
    }

    // Pool ���� Object�� �����ҽ� �����ϱ�
    private Unit CreateNewObject(int _arrIdx)
    {
        Unit obj = Instantiate(poolArr[_arrIdx].unit, poolArr[_arrIdx].transform);
        obj.gameObject.SetActive(false);
        return obj;
    }



}
