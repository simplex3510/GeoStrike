using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : MonoBehaviour
{
    public bool isEmty = true;

    public Vector2 tileCoordinate; // ÁÂÇ¥ °ª

    private SpriteRenderer spriteRenderer;

    public Vector2 tileCoord { get { return tileCoordinate; } set { tileCoordinate = value; } }

    private void Awake()
    {
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
    }

    public void SetColor()
    {
        spriteRenderer.color = Color.blue;
    }
}
