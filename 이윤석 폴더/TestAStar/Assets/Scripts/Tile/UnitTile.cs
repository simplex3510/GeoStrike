using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : MonoBehaviour
{
    public bool isEmty = true;

    public Vector2 tileCoordinate; // 좌표 값
    public Vector3 worldPos; // 월드 위치 값
    public Unit unit = null; // 이 타일에 배치된 유닛

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
