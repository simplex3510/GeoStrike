using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr : MonoBehaviour
{
    public static ObjectPoolMgr instance;

    public UnitCreation unitCreator;

    public Queue<Unit> poolingObjectQueue = new Queue<Unit>();
    //public Pool[] poolArr; 

    private void Awake()
    {
        instance = this;
    }

    virtual protected void InitObjectPool(int _num, int _idx)
    {
        for(int idx = 0; idx < _num; idx++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject(_idx));
        }
    }

    // Pool 내부 Object가 부족할시 증식하기
    private Unit CreateNewObject(int _idx)
    {
        Unit obj = Instantiate(GetUnit(_idx), transform);
        obj.gameObject.SetActive(false);
        return obj;
    }

    // Pool에서 유닛 가져오기
    public Unit GetObject(Unit _unit, int _idx)
    {
        if (instance.poolingObjectQueue.Count > 0)
        {
            Unit obj = instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            Unit newObj = instance.CreateNewObject(_idx);
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    // 사용된 Object를 Pool로 반환
    private void ReturnObject(Unit _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(instance.transform);
        instance.poolingObjectQueue.Enqueue(_obj);
    }

    protected Unit GetUnit(int _idx)
    {
        Unit unit = GameMgr.instance.tetrtominoList[_idx].gameObject.GetComponent<UnitCreation>().unit;
        return unit;
    }
}
