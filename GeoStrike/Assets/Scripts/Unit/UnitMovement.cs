using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public Rigidbody2D rigid2D;

    private void Awake()
    {
        if (rigid2D == null) { rigid2D = GetComponent<Rigidbody2D>(); }
    }

    public void Move()
    {
        rigid2D.MovePosition(transform.position + new Vector3(1, 0));
    }
}
