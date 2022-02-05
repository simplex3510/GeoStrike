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

    // 초기 Object 생성
    private void InitObjectPool(int _num)
    {
        for (int idx = 0; idx < _num; idx++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    // Pool에 NewObject 생성
    private Unit CreateNewObject()
    {
        Unit obj = Instantiate(unit, transform);
        obj.gameObject.SetActive(false);
        return obj;
    }

    // Pool에서 유닛 가져오기
    public Unit GetObject()
    {
        // Pool에 Object가 있을 경우
        if (poolingObjectQueue.Count > 0)
        {
            Unit obj = poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        // Pool에 Object가 부족 할 경우
        else
        {
            Unit newObj = CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    // 사용된 Object를 Pool로 반환
    private void ReturnObject(Unit _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(_obj);
    }
}
