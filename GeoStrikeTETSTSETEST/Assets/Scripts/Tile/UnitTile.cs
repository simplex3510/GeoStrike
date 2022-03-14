using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : MonoBehaviour
{
    public Vector3 worldPos; // 월드 위치 값

    private void Awake()
    {
        worldPos = transform.TransformPoint(Vector3.zero);
    }
}
