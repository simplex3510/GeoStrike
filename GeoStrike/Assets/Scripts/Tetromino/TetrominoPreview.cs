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
        m_cameraController.mouseController.eMouseMode = MouseController.EMouseMode.normal;  // ���콺 ��� : �븻
        transform.position = m_previewOriginPos;                     // ������ ��ġ �ʱ�ȭ
        m_previewImage.sprite = null;                                // �̹��� �ʱ�ȭ
        rectTransform.rotation = Quaternion.identity;                // ȸ���� �ʱ�ȭ
        rectTransform.sizeDelta = m_previewOriginSize;               // ������ �ʱ�ȭ
        clickSlot = null;                                            // Ŭ�� ���� ���� �ʱ�ȭ
        m_cameraController.mouseController.CursorVisible(true);      // ���콺 Ŀ�� �ʱ�ȭ
    }
}
