using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OMino : Tetromino
{
    [SerializeField] private int ShapeIndex = 0;
    private Vector2[] O = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

    public override Vector2[] GetCoordinate()
    {
        coordinate = O;
        return coordinate;
    }
}
