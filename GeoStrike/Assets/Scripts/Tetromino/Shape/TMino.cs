using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMino : Tetromino
{
    private Vector2[] T = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1) };

    public override Vector2[] GetCoordinate()
    {
        coordinate = T;
        return coordinate;
    }
}
