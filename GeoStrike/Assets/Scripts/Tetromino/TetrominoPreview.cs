using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrominoPreview : MonoBehaviour
{
    [SerializeField] private CameraController m_cameraController;

    public static TetrominoPreview instance;

    public Image m_previewImage;
    public TetrominoSlot m_clickSlot;
    public Vector3 m_previewOriginPos;

    // Preview size controll
    public RectTransform m_rectTransform;
    public Vector2 m_previewOriginSize;

    private void Start()
    {
        instance = this;
        m_previewOriginPos = transform.position;
        m_previewOriginSize = m_rectTransform.sizeDelta;
    }

    public void ClearPreview()
    {
        m_cameraController.m_mouseController.m_mouseMode = MouseController.E_MouseMode.normal;
        transform.position = m_previewOriginPos;
        m_previewImage.sprite = null;
        m_rectTransform.sizeDelta = m_previewOriginSize;
        instance.m_clickSlot = null;
        m_cameraController.m_mouseController.CursorVisible(true);
    }
}
