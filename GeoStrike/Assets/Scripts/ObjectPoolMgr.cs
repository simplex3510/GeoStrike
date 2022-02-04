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

    // 초기 Object 생성
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

    // Pool 내부 Object가 부족할시 증식하기
    private Unit CreateNewObject(int _arrIdx)
    {
        Unit obj = Instantiate(poolArr[_arrIdx].unit, poolArr[_arrIdx].transform);
        obj.gameObject.SetActive(false);
        return obj;
    }



}
