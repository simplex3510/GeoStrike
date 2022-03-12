using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    // 0 : Tetromino, 1 : Unit, 2: Tower
    public StatusInfo[] clickedObjectArr = new StatusInfo[3];

    private void Awake()
    {
        if (clickedObjectArr == null) { clickedObjectArr = GetComponentsInChildren<StatusInfo>(); }
    }

    public void SetActiveFalseAll()
    {
        foreach (StatusInfo info in clickedObjectArr)
        {
            info.gameObject.SetActive(false);
        }
    }
}
