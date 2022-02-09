using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TetrominoTile : MonoBehaviour
{
    public bool isEmty = true;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
    }

    private void Update()
    {
        SetColor();
    }

    public void SetColor()
    {
        if (!isEmty)
        {
            spriteRenderer.color = Color.black;
        }
    }
}
