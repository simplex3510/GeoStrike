using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMino : Tetromino
{
    [SerializeField] private int ShapeIndex = 4;
    private Vector2[] L = { new Vector2(0, 1), new Vector2(0, 0), new Vector2(0, -1), new Vector2(1, -1) };

    public override Vector2[] GetCoordinate()
    {
        coordinate = L;
        return coordinate;
    }
}
