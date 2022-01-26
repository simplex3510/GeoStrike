using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_mainCamera;
    public MouseController m_mouseController;

    // Camera Movement by mouse Position
    [SerializeField] private RectTransform m_rectTransform;     // Resolution 1920 x 1080
    [SerializeField] private float m_cameraSpeed;
    private readonly int ZERO = 0;

    // Camera Zoom
    [SerializeField] float m_zoomSpeed = 0f;
    [SerializeField] float m_zoomIn = 0f;
    [SerializeField] float m_zoomOut = 0f;

    private void Awake()
    {
        if (m_mainCamera == null) { m_mainCamera = GetComponent<Camera>(); }
        if (m_mouseController == null) { m_mouseController = GetComponent<MouseController>(); }
    }

    private void Update()
    {
        CameraMovement();
        CameraZoom();
    }

    private void CameraMovement()
    {
        Vector3 velocity;
    
        // Camera move horizotnal
        if ((m_mouseController.m_mousePos.x >= m_rectTransform.rect.width) || (Input.GetKey(KeyCode.RightArrow)))
        {
            velocity = transform.position + Vector3.right;
            transform.position = Vector3.Lerp(transform.position, velocity, m_cameraSpeed);
        }
        else if (m_mouseController.m_mousePos.x <= ZERO || (Input.GetKey(KeyCode.LeftArrow)))
        {
            velocity = transform.position - Vector3.right;
            transform.position = Vector3.Lerp(transform.position, velocity, m_cameraSpeed);
        }

        // Camera move vertical
        if (m_mouseController.m_mousePos.y >= m_rectTransform.rect.height || (Input.GetKey(KeyCode.UpArrow)))
        {
            velocity = transform.position + Vector3.up;
            transform.position = Vector3.Lerp(transform.position, velocity, m_cameraSpeed);
        }
        else if (m_mouseController.m_mousePos.y <= ZERO || (Input.GetKey(KeyCode.DownArrow)))
        {
            velocity = transform.position - Vector3.up;
            transform.position = Vector3.Lerp(transform.position, velocity, m_cameraSpeed);
        }
    }

    private void CameraZoom()
    {
        float zoomDir = Input.GetAxis("Mouse ScrollWheel");
        float currentSize = m_mainCamera.orthographicSize - zoomDir * m_zoomSpeed;

        m_mainCamera.orthographicSize = Mathf.Clamp(currentSize, m_zoomIn, m_zoomOut);
    }

    
}
