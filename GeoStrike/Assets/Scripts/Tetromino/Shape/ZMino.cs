using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZMino : Tetromino
{
    private Vector2[] Z = { new Vector2(0, 1), new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1) };

    public override Vector2[] GetCoordinate()
    {
        coordinate = Z;
        return coordinate;
    }
}
