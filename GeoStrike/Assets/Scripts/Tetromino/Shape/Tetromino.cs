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
    [HideInInspector] public EShapeIndex shapeIdx;
    [HideInInspector] public string shapeName;
    public int cost;    // 비용

    [HideInInspector] public Quaternion quaternion;                     // 회전 값
    [HideInInspector] public Vector2[] coordinate = new Vector2[4];     // 좌표
    
    public abstract Vector2[] GetCoordinate();

    private void Awake()
    {
        shapeName = shapeIdx.ToString() + " Mino";
    }
}
