using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TetrominoTile : MonoBehaviour
{
    public bool isEmty = true;
    public SpriteRenderer image;

    private void Awake()
    {
        if (image == null) { image = GetComponent<SpriteRenderer>(); }
    }

    private void Update()
    {
        SetColor();
    }

    public void SetColor()
    {
        if (!isEmty)
        {
            image.color = Color.black;
        }
    }
}
