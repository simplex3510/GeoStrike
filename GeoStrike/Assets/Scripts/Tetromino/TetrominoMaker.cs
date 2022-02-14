using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoMaker : MonoBehaviour
{
    public List<Tetromino> tetrominoList = new List<Tetromino>(); // 7���� ���

    public TetrominoSlot tetrominoSlot;
    private GameObject tetrominoObj;

    private int randomShape;     // ���� ���
    private int randomRotation;   // ���� ȸ�� ��
    private Vector3 angle;

    private int[] angleArr = { 0, 90, 180, 270 };

    private void Awake()
    {
        if (tetrominoSlot == null) { tetrominoSlot = GetComponentInParent<TetrominoSlot>(); }
    }

    private void Start()
    {
        RandomTetromino();
    }

    // Coordinate DebugTest
    private void Debg()
    {
        for (int i = 0; i < tetrominoList.Count; i++)
        {
            for(int idx = 0; idx < 4; idx++)
            {
                Debug.Log(tetrominoList[i] + " : " + tetrominoList[i].GetCoordinate()[idx]);
            }
        }
    }

    public GameObject GetTetrominoObj()
    {
        return tetrominoObj;
    }

    public Tetromino GetTetromino()
    {
        return tetrominoObj.GetComponent<Tetromino>();
    }

    public void RandomTetromino()
    {
        tetrominoSlot.showSlotImage.rectTransform.rotation = Quaternion.identity; // �ʱ�ȭ

        randomShape = Random.Range(0, 7);  //   7���� ���
        randomRotation = Random.Range(0, 4);

        tetrominoObj = tetrominoList[randomShape].gameObject;   // ���Կ� ���� ��Ʈ�ι̳� ���� ����
        tetrominoSlot.showSlotImage.rectTransform.Rotate(GetAngle());   // ���� �̹��� ȸ��
        tetrominoSlot.showSlotImage.sprite = tetrominoObj.GetComponent<SpriteRenderer>().sprite;  // ���� �̹��� ����
    }

    // ȸ����Theta ����
    public Vector3 GetAngle()
    {
        int rotZ = angleArr[randomRotation];

        switch (randomShape)
        {
            // O
            case 0:
                angle = Vector3.zero;   // �̰� �� �ϴϱ� ���� �̻�����
                break;
            // I
            case 1:
                switch (randomRotation)
                {
                    case 0:
                    case 3:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 1:
                    case 4:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                }
                break;
            // T
            case 2:
                switch (randomRotation)
                {
                    case 0:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 1:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 2:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 3:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                }
                break;
            // J
            case 3:
                switch (randomRotation)
                {
                    case 0:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 1:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 2:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 3:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                }
                break;
            // L
            case 4:
                switch (randomRotation)
                {
                    case 0:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 1:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 2:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 3:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                }
                break;
            // S
            case 5:
                switch (randomRotation)
                {
                    case 0:
                    case 3:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 1:
                    case 4:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                }
                break;
            // Z
            case 6:
                switch (randomRotation)
                {
                    case 0:
                    case 3:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                    case 1:
                    case 4:
                        angle = new Vector3(0f, 0f, rotZ);
                        break;
                }
                break;
        }
        return angle;
    }
}
