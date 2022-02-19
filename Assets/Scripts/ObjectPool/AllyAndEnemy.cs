using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAndEnemy : MonoBehaviour
{
    public Pool[] poolArr = new Pool[6];

    private void Awake()
    {
        if (poolArr == null) { poolArr = GetComponentsInChildren<Pool>(); }
    }
}