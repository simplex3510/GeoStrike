using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    // 0 : Tetromino, 1 : Unit, 2: Tower
    public StatusInfo[] statusInfoArr = new StatusInfo[3];

    private void Awake()
    {
        statusInfoArr = GetComponentsInChildren<StatusInfo>();

        SetActiveFalseAll();
    }

    public void SetActiveFalseAll()
    {
        foreach (StatusInfo info in statusInfoArr)
        {
            info.gameObject.SetActive(false);
        }
    }
}
