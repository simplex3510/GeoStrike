using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySlotPanel : MonoBehaviour
{
    public KeySlotInfo[] keySlotArr = new KeySlotInfo[2];

    private void Awake()
    {
        keySlotArr = GetComponentsInChildren<KeySlotInfo>();
    }

    public void SetActiveFalseAll()
    {
        foreach (KeySlotInfo info in keySlotArr)
        {
            info.gameObject.SetActive(false);
        }
    }
}
