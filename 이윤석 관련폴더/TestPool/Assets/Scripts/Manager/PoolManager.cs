using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public ObjectPool[] objectPools;

    private void Awake()
    {
        objectPools = GetComponentsInChildren<ObjectPool>();
    }
}
