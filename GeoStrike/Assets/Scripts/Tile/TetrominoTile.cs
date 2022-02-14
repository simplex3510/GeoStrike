using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TetrominoTile : MonoBehaviour
{
    public bool isEmty = true;

    public Vector2 tileCoordinate; // ÁÂÇ¥ °ª

    private SpriteRenderer spriteRenderer;

    public Vector2 tileCoord { get { return tileCoordinate; } set { tileCoordinate = value; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isEmty)
        {
            spriteRenderer.color = Color.black;
        }
    }
}
