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
    [SerializeField] public TetrominoMaker tetrominoMaker;
    [HideInInspector] public RectTransform rectSlot;

    // 프리뷰 이미지와 슬롯 이미지 동기화 변수
    [HideInInspector]public Image slotImage;

    // Tile의 위치, 사이즈 저장
    [HideInInspector] public Vector3 tilePos;
    [HideInInspector] public Vector3 tileSize;

    // Size
    [HideInInspector] public Vector2 tetrominoImgSize;
    [HideInInspector] public Vector3 offset;

    private void Awake()
    {
        if (tetrominoMaker == null) { tetrominoMaker = GetComponent<TetrominoMaker>(); }
        if (rectSlot == null) { rectSlot = GetComponent<RectTransform>(); }
    }

    // When choice slot.
    public void ChoiceTetrominoSlot()
    {
        TetrominoPreview.instance.clickSlot = this;
        TetrominoPreview.instance.m_previewImage.sprite = slotImage.sprite;
        TetrominoPreview.instance.rectTransform.Rotate(tetrominoMaker.GetAngle());
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.build;

        tileDetector.tetrominoObj = tetrominoMaker.GetTetrominoObj();
        tileDetector.tetromino = tetrominoMaker.GetTetromino();
        tileDetector.angle = tetrominoMaker.GetAngle();

        StartCoroutine(CTetrominoPreviewPos());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ChoiceTetrominoSlot();
            cameraController.autoMoveCamera.MoveToBuildZone();
            PreviewImgSize();

            cameraController.mouseController.CursorVisible(false);
        }
    }

    // Set preview size
    private void PreviewImgSize()
    {
        tetrominoImgSize = slotImage.sprite.rect.size;
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

    IEnumerator CCancel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetMouseButtonDown(MouseController.CLICK_RIGHT)))
        {
            TetrominoPreview.instance.ClearPreview();

            cameraController.mouseController.CursorVisible(true);
        }
        yield return null;
    }

    IEnumerator CTetrominoPreviewPos()
    {
        while (cameraController.mouseController.eMouseMode == MouseController.EMouseMode.build)
        {
            TetrominoPreview.instance.transform.position = Camera.main.WorldToScreenPoint(Get_TilePos(tileDetector.tile));

            StartCoroutine(CCancel());
            yield return null;
        }
    }
}
