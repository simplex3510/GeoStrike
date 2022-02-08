using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public Rigidbody2D rigid2D;
    public Unit unit;

    private void Start()
    {
        if (rigid2D == null) { rigid2D = GetComponent<Rigidbody2D>(); }
        if (unit == null) { unit = GetComponent<Unit>(); }
    }

    public void Move()
    {
        rigid2D.MovePosition(rigid2D.position + new Vector2(1, 0) * unit.unitInfo.movementSpeed * Time.deltaTime);
    }
}
