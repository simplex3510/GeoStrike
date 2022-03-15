using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSlotContainer : MonoBehaviour
{
    public TetrominoSlot[] slotArr = new TetrominoSlot[2];

    private void Awake()
    {
        slotArr = GetComponentsInChildren<TetrominoSlot>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int idx = 0; idx < slotArr.Length; idx++)
            {
                slotArr[idx].tetrominoMaker.RandomTetromino();
            }
        }
    }
}
