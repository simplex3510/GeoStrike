using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum EShapeIndex
{
    I = 0,
    T,
    J,
    L,
    S,
    Z,
    O
}

public abstract class Tetromino : MonoBehaviour
{
    public EShapeIndex shapeIdx;
    public string shapeName;
    public Quaternion quaternion;
    public int cost;
    public Vector2[] coordinate = new Vector2[4];
    
    public abstract Vector2[] GetCoordinate();

    private void Awake()
    {
        shapeName = shapeIdx.ToString() + " Mino";
    }
}
