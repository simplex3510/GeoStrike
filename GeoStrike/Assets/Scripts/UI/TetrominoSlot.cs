using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TetrominoSlot : MonoBehaviour, IPointerClickHandler
{
    // Components
    [SerializeField] private CameraController m_cameraController;
    [SerializeField] private ObjectDetecter m_objDetecter;

    // Random block
    public Image m_buildImage;
    public GameObject m_tetrominoPrefab;

    public int m_currentBlockShape;
    public int m_currentBlockRotation;

    public Vector3 m_currentBlockAngle;
    public Vector3 m_tilePos;
    public Vector3 m_tileSize;

    // Size
    public Vector2 m_tetrominoImgSize;
    public Vector3 m_offset;

    private void Start()
    {
        RandomTetromino();
    }

    // When choice slot.
    public void ChoiceTetromino()
    {
        TetrominoPreview.instance.m_clickSlot = this;
        TetrominoPreview.instance.m_previewImage.sprite = m_buildImage.sprite;
        m_cameraController.m_mouseController.m_mouseMode = MouseController.E_MouseMode.create;
        StartCoroutine(IE_TetrominoPreviewPos());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ChoiceTetromino();
            PreviewImgSize();

            m_cameraController.m_mouseController.CursorVisible(false);
        }
    }

    // Set preview size
    private void PreviewImgSize()
    {
        m_tetrominoImgSize = m_buildImage.sprite.rect.size;
        TetrominoPreview.instance.m_rectTransform.sizeDelta = TetrominoPreview.instance.m_clickSlot.m_tetrominoImgSize * 2.15f;
    }

    public Vector3 Get_TilePos(Tile _tile)
    {
        if (_tile == null) { m_tilePos = Vector3.zero; }
        else
        {
            m_tilePos = _tile.transform.position;
        }
        return m_tilePos;
    }

    public void RandomTetromino()
    {
        int RandShape = Random.Range(0, 7);
        m_currentBlockShape = RandShape;

        switch (RandShape)
        {
            // 'け' - Has 0 type
            case 0:
                m_currentBlockRotation = 0;
                break;
            // 'び' - Has 2types
            case 1:
                m_currentBlockRotation = Random.Range(0, 2);
                break;
            // 'た' - Has 4types
            case 2:
                m_currentBlockRotation = Random.Range(0, 4);
                break;
            // 'い' - Has 4types
            case 3:
                m_currentBlockRotation = Random.Range(0, 4);
                break;
            // '-い' - Has 4types
            case 4:
                m_currentBlockRotation = Random.Range(0, 4);
                break;
            // 'Z' - Has 2types
            case 5:
                m_currentBlockRotation = Random.Range(0, 2);
                break;
            // '-Z' - Has 2types
            case 6:
                m_currentBlockRotation = Random.Range(0, 2);
                break;
        }

        // Random tetromino data
        m_tetrominoPrefab = GameMgr.instance.m_tetrtominoList[m_currentBlockShape].GetComponent<Tetromino>().m_rotateTetrominoObjList[m_currentBlockRotation];
        m_buildImage.sprite = m_tetrominoPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    IEnumerator IE_Cancel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetMouseButtonDown(1)))
        {
            TetrominoPreview.instance.ClearPreview();

            m_cameraController.m_mouseController.CursorVisible(true);
        }
        yield return null;
    }

    IEnumerator IE_TetrominoPreviewPos()
    {
        while (m_cameraController.m_mouseController.m_mouseMode == MouseController.E_MouseMode.create)
        {
            TetrominoPreview.instance.transform.position = Camera.main.WorldToScreenPoint(Get_TilePos(m_objDetecter.m_tile)) - PreviewPosOffset();

            StartCoroutine(IE_Cancel());
            yield return null;
        }
    }

    public Vector3 PreviewPosOffset()
    {
        m_offset = new Vector3(53f, 51f);
        return m_offset;
    }
}
