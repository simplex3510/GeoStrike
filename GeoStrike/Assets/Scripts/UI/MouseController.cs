using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public enum EMouseMode
    {
        normal,
        create,
        batch
    }

    [HideInInspector] public Vector3 mousePos;
    private EMouseMode eMouseMode;

    private void Update()
    {
        mousePos = Input.mousePosition;
    }

    public void SetMode(EMouseMode _mode)
    {
        eMouseMode = _mode;
    }

    public EMouseMode GetMode()
    {
        return eMouseMode;
    }
    
    public void CursorVisible(bool _bool)
    {
        Cursor.visible = _bool;
    }
}
