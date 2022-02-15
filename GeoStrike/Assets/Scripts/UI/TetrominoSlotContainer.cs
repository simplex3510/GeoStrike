using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSlotContainer : MonoBehaviour
{
    public List<TetrominoSlot> slotList = new List<TetrominoSlot>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int idx = 0; idx < slotList.Count; idx++)
            {
                slotList[idx].tetrominoMaker.RandomTetromino();
            }
        }
    }
}
