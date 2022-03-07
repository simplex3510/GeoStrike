using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JMino : Tetromino
{
    [SerializeField] private int ShapeIndex = 3;
    private Vector2[] J = { new Vector2(0, 1), new Vector2(0, 0), new Vector2(0, -1), new Vector2(-1, -1) };

    public override Vector2[] GetCoordinate()
    {
        coordinate = J;
        return coordinate;
    }
}
