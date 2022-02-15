using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tetromino : MonoBehaviour
{
    public Vector2[] coordinate;

    public abstract Vector2[] GetCoordinate();
}
