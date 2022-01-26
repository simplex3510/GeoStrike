using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TetrominoTile : MonoBehaviour
{
    public bool m_IsBuilding { get; set; }
    public SpriteRenderer m_Image;

    private void Awake()
    {
        if (m_Image == null) { m_Image = GetComponent<SpriteRenderer>(); }
        m_IsBuilding = false;
    }

    private void Update()
    {
        Set_color(m_IsBuilding);
    }

    public void Set_color(bool _bool)
    {
        if (m_IsBuilding)
        {
            m_Image.color = Color.black;
        }
    }
}
