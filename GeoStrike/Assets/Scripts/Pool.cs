using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : ObjectPoolMgr
{
    public Queue<Unit> poolingObjectQueue = new Queue<Unit>();

    // Pool에서 유닛 가져오기
    //public Unit GetObject(int _arrIdx)
    //{
    //    if (poolingObjectQueue.Count > 0)
    //    {
    //        Unit obj = poolingObjectQueue.Dequeue();
    //        obj.transform.SetParent(null);
    //        obj.gameObject.SetActive(true);
    //        return obj;
    //    }
    //    else
    //    {
    //        Unit newObj = CreateNewObject(_arrIdx);
    //        newObj.transform.SetParent(null);
    //        newObj.gameObject.SetActive(true);
    //        return newObj;
    //    }
    //}

    //// 사용된 Object를 Pool로 반환
    //private void ReturnObject(Unit _obj)
    //{
    //    _obj.gameObject.SetActive(false);
    //    _obj.transform.SetParent(transform);
    //    poolingObjectQueue.Enqueue(_obj);
    //}
}
