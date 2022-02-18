using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMino : Tetromino
{
    private Vector2[] I = { new Vector2(0, 1), new Vector2(0, 0), new Vector2(0, -1), new Vector2(0, -2) };

    public override Vector2[] GetCoordinate()
    {
        coordinate = I;
        return coordinate;
    }
}
