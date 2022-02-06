using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public Pool[] poolArr;


    private void Awake()
    {
        instance = this;

        poolArr = GetComponentsInChildren<Pool>();
    }
}
