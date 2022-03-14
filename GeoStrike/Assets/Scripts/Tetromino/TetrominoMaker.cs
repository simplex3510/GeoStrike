using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


[DefaultExecutionOrder(203)]
public class TetrominoMaker : MonoBehaviourPun
{
    [HideInInspector] public Tetromino[] tetrominoArr = new Tetromino[7]; // 7���� ����� ��� �迭

    [HideInInspector] public TetrominoSlot tetrominoSlot;
    private GameObject tetrominoObj;

    private int randomShape;     // ���� ��� idx
    private int randomRotation;   // ���� ȸ�� idx
    private Vector3 angle;

    private int[] angleArr = { 0, 90, 180, 270 };

    private char[] alphbet = { 'O', 'I', 'T', 'J', 'L', 'S', 'Z' };

    private void Awake()
    {
        if (tetrominoSlot == null) { tetrominoSlot = GetComponentInParent<TetrominoSlot>(); }

        InitTetrominoSprite();
        InitTetrominoArr();
    }

    private void Start()
    {
        RandomTetromino();
    }

    public void RandomTetromino()
    {
        tetrominoSlot.rectSlot.rotation = Quaternion.identity; // ȸ���� �ʱ�ȭ

        randomShape = Random.Range(0, 7);  //   7������ ���
        randomRotation = Random.Range(0, 4);    // 4������ ȸ����

        tetrominoObj = tetrominoArr[randomShape].gameObject;   // ���Կ� �������� ���� ��Ʈ�ι̳� Obj ����

        tetrominoSlot.rectSlot.Rotate(GetAngle());   // ���� �̹��� ȸ��
        tetrominoSlot.slotImage.sprite = tetrominoObj.GetComponent<SpriteRenderer>().sprite;  // ���� �̹��� ����

        // ������ �ؽ�Ʈ 
        tetrominoSlot.costText.text = tetrominoObj.GetComponent<Tetromino>().cost.ToString();
        tetrominoSlot.slotInfoText.text = tetrominoObj.GetComponent<Tetromino>().shapeIdx + " Mino : " + tetrominoObj.GetComponent<Tetromino>().unitIdx;     
    }

    public GameObject GetTetrominoObj()
    {
        return tetrominoObj;
    }

    public Tetromino GetTetromino()
    {
        return tetrominoObj.GetComponent<Tetromino>();
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

    private void InitTetrominoArr()
    {
        for (int shapeIdx = 0; shapeIdx < ArrayNumber.TETROMINO_SHAPE; shapeIdx++)
        {
            if (GameMgr.isMaster)
            {
                tetrominoArr[shapeIdx] = Resources.Load<Tetromino>("Tetromino/Prefabs/BlueTeam/Blue_" + alphbet[shapeIdx]);
            }
            else
            {
                tetrominoArr[shapeIdx] = Resources.Load<Tetromino>("Tetromino/Prefabs/RedTeam/Red_" + alphbet[shapeIdx]);
            }
        }
    }

    private void InitTetrominoSprite()
    {
        for (int shapeIdx = 0; shapeIdx < ArrayNumber.TETROMINO_SHAPE; shapeIdx++)
        {
            Resources.Load<GameObject>("Tetromino/Prefabs/BlueTeam/Blue_" + alphbet[shapeIdx]).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tetromino/Sprites/BlueTeam/Blue_" + alphbet[shapeIdx]);
            Resources.Load<GameObject>("Tetromino/Prefabs/RedTeam/Red_" + alphbet[shapeIdx]).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tetromino/Sprites/RedTeam/Red_" + alphbet[shapeIdx]);
        }
    }
}
