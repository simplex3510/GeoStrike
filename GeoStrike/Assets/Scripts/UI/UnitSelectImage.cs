using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(202)]
public class UnitSelectImage : MonoBehaviour
{
    public Vector3 originPos;

    private void Awake()
    {
        originPos = this.transform.position;
    }
}
