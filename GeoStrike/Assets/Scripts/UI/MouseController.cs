using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public enum EMouseMode
    {
        normal,
        create
    }

    [HideInInspector] public Vector3 mousePos;
    public EMouseMode eMouseMode;

    private void Update()
    {
        mousePos = Input.mousePosition;
    }

    public void CursorVisible(bool _bool)
    {
        Cursor.visible = _bool;
    }
}
