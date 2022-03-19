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
    public EUnitIndex unitIdx;
    [HideInInspector] public string shapeName;
    public int cost;    // ���

    [HideInInspector] public Quaternion quaternion;                     // ȸ�� ��
    [HideInInspector] public Vector2[] coordinate = new Vector2[4];     // ��ǥ
    
    public abstract Vector2[] GetCoordinate();

    private void Awake()
    {
        shapeName = shapeIdx.ToString() + " Mino";
    }
}
