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
        m_cameraController.mouseController.eMouseMode = MouseController.EMouseMode.normal;  // 마우스 모드 : 노말
        transform.position = m_previewOriginPos;                     // 프리뷰 위치 초기화
        m_previewImage.sprite = null;                                // 이미지 초기화
        rectTransform.rotation = Quaternion.identity;                // 회전값 초기화
        rectTransform.sizeDelta = m_previewOriginSize;               // 사이즈 초기화
        clickSlot = null;                                            // 클릭 슬롯 정보 초기화
        m_cameraController.mouseController.CursorVisible(true);      // 마우스 커서 초기화
    }
}
