using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(204)]
public class TetrominoSlot : MonoBehaviour
{
    // Auto input components
    [HideInInspector] private CameraController cameraController;
    [HideInInspector] private Detector detector;
    [HideInInspector] public TetrominoMaker tetrominoMaker;
    [HideInInspector] public RectTransform rectSlot;
    [HideInInspector] private Button button;

    // �ڿ����� ���� �̹��� ���� ��ȭ
    [Header("< Manual Input Components >")]
    [SerializeField] private Image image;

    // ������ �̹����� ���� �̹��� �ʱ�ȭ ����
    [HideInInspector] public Image slotImage;

    // Tile�� ��ġ, ������ ����
    [HideInInspector] public Vector3 tilePos;
    [HideInInspector] public Vector3 tileSize;

    // Size
    [HideInInspector] public Vector2 tetrominoImgSize;
    [HideInInspector] public Vector3 offset;

    // ���� ������ ��Ʈ�ι̳�, ���� �ؽ�Ʈ
    public Text slotInfoText;
    public Text costText;

    private void Awake()
    {
        if (cameraController == null) { cameraController = GameObject.FindObjectOfType<CameraController>(); }
        if (detector == null) { detector = GameObject.FindObjectOfType<Detector>(); }
        if (tetrominoMaker == null) { tetrominoMaker = GetComponent<TetrominoMaker>(); }
        if (rectSlot == null) { rectSlot = GetComponent<RectTransform>(); }
        if (button == null) { button = GetComponentInChildren<Button>(); }
    }

    private void Start()
    {
        StartCoroutine(EOnOFFButton());
    }


    // When choice slot.
    public void ChoiceTetrominoSlot()
    {
        TetrominoPreview.instance.clickSlot = this;
        TetrominoPreview.instance.m_previewImage.sprite = slotImage.sprite;
        TetrominoPreview.instance.rectTransform.Rotate(tetrominoMaker.GetAngle());
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.build;

        detector.tetrominoObj = tetrominoMaker.GetTetrominoObj();
        detector.tetromino = tetrominoMaker.GetTetromino();
        detector.angle = tetrominoMaker.GetAngle();

        StartCoroutine(CTetrominoPreviewPos());
    }

    public void ClickSlot()
    {
        ChoiceTetrominoSlot();
        cameraController.autoMoveCamera.MoveToBuildZone();
        PreviewImgSize();

        cameraController.mouseController.CursorVisible(false);
    }

    // Set preview size
    private void PreviewImgSize()
    {
        tetrominoImgSize = slotImage.sprite.rect.size;
        TetrominoPreview.instance.rectTransform.sizeDelta = TetrominoPreview.instance.clickSlot.tetrominoImgSize * 1.8f;
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
            TetrominoPreview.instance.transform.position = Camera.main.WorldToScreenPoint(Get_TilePos(detector.tile));

            StartCoroutine(CCancel());
            yield return null;
        }
    }

    IEnumerator EOnOFFButton()
    {
        // �ڿ��� �����ϸ� ��ư ��Ȱ��ȭ
        while (true)
        {
            if (Geo.CurrentGeo < tetrominoMaker.GetTetrominoObj().GetComponent<Tetromino>().cost)
            {
                if (button.interactable == true)
                {
                    button.interactable = false;
                    image.color = new Color(60 / 255, 60 / 255, 60 / 255);
                }
            }
            else
            {
                if (button.interactable == false)
                {
                    button.interactable = true;
                    image.color = new Color(1, 1, 1);
                }
            }
            yield return null;
            yield return null;
        }
    }
}
