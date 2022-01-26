using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public enum E_MouseMode
    {
        normal,
        create
    }

    public Vector3 m_mousePos;
    public E_MouseMode m_mouseMode;

    private void Update()
    {
        m_mousePos = Input.mousePosition;
    }

    public void CursorVisible(bool _bool)
    {
        Cursor.visible = _bool;
    }
}
