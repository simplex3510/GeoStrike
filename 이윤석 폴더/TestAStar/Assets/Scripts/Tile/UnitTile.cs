using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : MonoBehaviour
{
    public bool isEmty = true;

    public Vector2 tileCoordinate; // ��ǥ ��
    public Vector3 worldPos; // ���� ��ġ ��
    public Unit unit = null; // �� Ÿ�Ͽ� ��ġ�� ����

    private SpriteRenderer spriteRenderer;

    public Vector2 tileCoord { get { return tileCoordinate; } set { tileCoordinate = value; } }

    private void Awake()
    {
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }

        worldPos = transform.TransformPoint(Vector3.zero);
    }

    public void SetColor()
    {
        spriteRenderer.color = Color.blue;
    }
}
