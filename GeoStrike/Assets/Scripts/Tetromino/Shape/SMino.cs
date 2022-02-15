using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMino : Tetromino
{
   private Vector2[] S = { new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1) };


   public override Vector2[] GetCoordinate()
   {
        coordinate = S;
        return coordinate;
    }
}
