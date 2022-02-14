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
    [SerializeField] private TetrominoMaker tetrominoMaker;

    [Header("< Random Block >")]
    public Image showSlotImage;

    [HideInInspector] public Vector3 tilePos;
    [HideInInspector] public Vector3 tileSize;

    // Size
    [HideInInspector] public Vector2 tetrominoImgSize;
    [HideInInspector] public Vector3 offset;

    // When choice slot.
    public void ChoiceTetromino()
    {
        TetrominoPreview.instance.clickSlot = this;
        TetrominoPreview.instance.m_previewImage.sprite = showSlotImage.sprite;
        cameraController.mouseController.SetMode(MouseController.EMouseMode.create);

        tileDetector.tetrominoObj = tetrominoMaker.GetTetrominoObj();
        tileDetector.tetromino = tetrominoMaker.GetTetromino();
        tileDetector.angle = tetrominoMaker.GetAngle();

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
        //tetrominoImgSize = forPreviewImage.sprite.rect.size;
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

  
    //private void ShowSlotImage(int _shape, int _rot) 
    //{
    //    showSlotImage.sprite = GameMgr.instance.tetrtominoList[currentBlockShape].GetComponent<Tetromino>().slotSprite;
    //    showSlotImage.GetComponent<RectTransform>().rotation = Quaternion.Euler(ImageRotation(_rot));
    //}

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
        while (cameraController.mouseController.GetMode() == MouseController.EMouseMode.create)
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
