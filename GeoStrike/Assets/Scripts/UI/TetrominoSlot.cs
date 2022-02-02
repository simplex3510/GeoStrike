using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TetrominoSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("< Components >")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Detector tileDetector;

    [Header("< Random Block >")]
    public Image showSlotImage;
    public Image forPreviewImage;
    public GameObject tetrominoPrefab;

    [HideInInspector] public int currentBlockShape;
    [HideInInspector] public int currentBlockRotation;

    [HideInInspector] public Vector3 imageAngle;
    [HideInInspector] public Vector3 tilePos;
    [HideInInspector] public Vector3 tileSize;

    // Size
    [HideInInspector] public Vector2 tetrominoImgSize;
    [HideInInspector] public Vector3 offset;

    private void Start()
    {
        RandomTetromino();
    }

    // When choice slot.
    public void ChoiceTetromino()
    {
        TetrominoPreview.instance.clickSlot = this;
        TetrominoPreview.instance.m_previewImage.sprite = forPreviewImage.sprite;
        cameraController.mouseController.emouseMode = MouseController.EMouseMode.create;
        StartCoroutine(CTetrominoPreviewPos());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ChoiceTetromino();
            PreviewImgSize();

            cameraController.mouseController.CursorVisible(false);
        }
    }

    // Set preview size
    private void PreviewImgSize()
    {
        tetrominoImgSize = forPreviewImage.sprite.rect.size;
        TetrominoPreview.instance.rectTransform.sizeDelta = TetrominoPreview.instance.clickSlot.tetrominoImgSize * 2.15f;
    }

    public Vector3 Get_TilePos(TetrominoTile _tile)
    {
        if (_tile == null) { tilePos = Vector3.zero; }
        else
        {
            tilePos = _tile.transform.position;
        }
        return tilePos;
    }

    public void RandomTetromino()
    {
        int RandShape = Random.Range(0, 7);
        currentBlockShape = RandShape;

        switch (RandShape)
        {
            // 'け' - Has 0 type
            case 0:
                currentBlockRotation = 0;
                break;
            // 'び' - Has 2types
            case 1:
                currentBlockRotation = Random.Range(0, 2);
                break;
            // 'た' - Has 4types
            case 2:
                currentBlockRotation = Random.Range(0, 4);
                break;
            // 'い' - Has 4types
            case 3:
                currentBlockRotation = Random.Range(0, 4);
                break;
            // '-い' - Has 4types
            case 4:
                currentBlockRotation = Random.Range(0, 4);
                break;
            // 'Z' - Has 2types
            case 5:
                currentBlockRotation = Random.Range(0, 2);
                break;
            // '-Z' - Has 2types
            case 6:
                currentBlockRotation = Random.Range(0, 2);
                break;
        }

        // Random tetromino data
        tetrominoPrefab = GameMgr.instance.tetrtominoList[currentBlockShape].GetComponent<Tetromino>().rotateTetrominoObjList[currentBlockRotation];
        ShowSlotImage(RandShape, currentBlockRotation);
        forPreviewImage.sprite = tetrominoPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    private Vector3 ImageRotation(int _rot)
    {
        switch (_rot)
        {
            case 0:
                imageAngle = Vector3.zero;
                break;
            case 1:
                imageAngle = new Vector3(0f, 0f, -90f);
                break;
            case 2:
                imageAngle = new Vector3(0f, 0f, -180f);
                break;
            case 3:
                imageAngle = new Vector3(0f, 0f, -270f);
                break;
        }  
        return imageAngle;
    }

    private void ShowSlotImage(int _shape, int _rot) 
    {
        showSlotImage.sprite = GameMgr.instance.tetrtominoList[currentBlockShape].GetComponent<Tetromino>().slotSprite;
        showSlotImage.GetComponent<RectTransform>().rotation = Quaternion.Euler(ImageRotation(_rot));
    }

    IEnumerator CCancel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetMouseButtonDown(1)))
        {
            TetrominoPreview.instance.ClearPreview();

            cameraController.mouseController.CursorVisible(true);
        }
        yield return null;
    }

    IEnumerator CTetrominoPreviewPos()
    {
        while (cameraController.mouseController.emouseMode == MouseController.EMouseMode.create)
        {
            TetrominoPreview.instance.transform.position = Camera.main.WorldToScreenPoint(Get_TilePos(tileDetector.tile)) - PreviewPosOffset();

            StartCoroutine(CCancel());
            yield return null;
        }
    }

    public Vector3 PreviewPosOffset()
    {
        offset = new Vector3(53f, 51f);
        return offset;
    }
}
