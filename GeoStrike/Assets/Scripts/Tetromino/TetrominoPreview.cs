using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrominoPreview : MonoBehaviour
{
    [SerializeField] private CameraController m_cameraController;

    public static TetrominoPreview instance;

    public Image m_previewImage;
    public TetrominoSlot clickSlot;
    [HideInInspector] public Vector3 m_previewOriginPos;

    // Preview size controll
    public RectTransform rectTransform;
    [HideInInspector] public Vector2 m_previewOriginSize;

    private void Start()
    {
        instance = this;
        m_previewOriginPos = transform.position;
        m_previewOriginSize = rectTransform.sizeDelta;
    }

    public void ClearPreview()
    {
        m_cameraController.mouseController.emouseMode = MouseController.EMouseMode.normal;
        transform.position = m_previewOriginPos;
        m_previewImage.sprite = null;
        rectTransform.sizeDelta = m_previewOriginSize;
        instance.clickSlot = null;
        m_cameraController.mouseController.CursorVisible(true);
    }
}
