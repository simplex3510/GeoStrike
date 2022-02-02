using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Pool instance;

    public UnitState unitPrefab;

    public Queue<UnitState> poolingObjectQueue = new Queue<UnitState>();

    private void Awake()
    {
        instance = this;
    }

    private UnitState CreateNewObject()
    {
        UnitState obj = Instantiate(unitPrefab, transform);
        obj.gameObject.SetActive(false);
        return unitPrefab;
    }

    public UnitState Get_Objeet()
    {
        if (instance.poolingObjectQueue.Count > 0)
        {
            UnitState obj = instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null); 
            obj.gameObject.SetActive(false);
            return obj;
        }
        else
        {
            UnitState newObj = instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    private void ReturnObject(UnitState _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(instance.transform);
        instance.poolingObjectQueue.Enqueue(_obj);
    }
}
