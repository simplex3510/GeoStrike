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

    // 초기 Object 생성
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

    // Pool에 NewObject 생성
    private Unit CreateNewObject()
    {
        Unit newObj = PhotonNetwork.Instantiate(unit.name, transform.position, Quaternion.identity).GetComponent<Unit>();
        return newObj;
    }

    // Pool에서 유닛 가져오기
    public Unit GetObject()
    {
        if (photonView.IsMine)
        {
            // Pool에 Object가 있을 경우
            if (p1ObjPoolQueue.Count > 0)
            {
                Debug.Log("get Object");
                Unit obj = p1ObjPoolQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            // Pool에 Object가 부족 할 경우
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
            // Pool에 Object가 있을 경우
            if (p2ObjPoolQueue.Count > 0)
            {
                Debug.Log("get Object");
                Unit obj = p2ObjPoolQueue.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            // Pool에 Object가 부족 할 경우
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

    // 사용된 Object를 Pool로 반환
    private void ReturnObject(Unit _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(transform);
        p1ObjPoolQueue.Enqueue(_obj);
    }

}
