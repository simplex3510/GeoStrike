using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public const int CLICK_LEFT = 0;
    public const int CLICK_RIGHT = 1;

    public enum EMouseMode
    {
        normal,
        build,
        batch
    }

    [HideInInspector] public Vector3 mousePos;
    private EMouseMode _eMouseMode;
    public EMouseMode eMouseMode { get { return _eMouseMode; } set { _eMouseMode = value; } }

    private void Update()
    {
        mousePos = Input.mousePosition;
    }

    public void CursorVisible(bool _bool)
    {
        Cursor.visible = _bool;
    }
}
