using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TetrominoTile : MonoBehaviour
{
    public bool isBuilding { get; set; }
    public SpriteRenderer image;

    private void Awake()
    {
        if (image == null) { image = GetComponent<SpriteRenderer>(); }
        isBuilding = false;
    }

    private void Update()
    {
        Set_color(isBuilding);
    }

    public void Set_color(bool _bool)
    {
        if (isBuilding)
        {
            image.color = Color.black;
        }
    }
}
