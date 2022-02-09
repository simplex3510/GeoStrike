using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : MonoBehaviour
{
    public bool isEmty = true;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
    }

    public void SetColor()
    {
        spriteRenderer.color = Color.blue;
    }
}
