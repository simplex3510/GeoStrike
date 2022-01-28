using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public MouseController mouseController;

    // Camera Movement by mouse Position
    [SerializeField] private RectTransform rectTransform;     // Resolution 1920 x 1080
    [SerializeField] private float cameraSpeed;
    private readonly int ZERO = 0;

    // Camera Zoom
    [SerializeField] float zoomSpeed = 0f;
    [SerializeField] float zoomIn = 0f;
    [SerializeField] float zoomOut = 0f;

    private void Awake()
    {
        if (mainCamera == null) { mainCamera = GetComponent<Camera>(); }
        if (mouseController == null) { mouseController = GetComponent<MouseController>(); }
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
        if ((mouseController.mousePos.x >= rectTransform.rect.width) || (Input.GetKey(KeyCode.RightArrow)))
        {
            velocity = transform.position + Vector3.right;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }
        else if (mouseController.mousePos.x <= ZERO || (Input.GetKey(KeyCode.LeftArrow)))
        {
            velocity = transform.position - Vector3.right;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }

        // Camera move vertical
        if (mouseController.mousePos.y >= rectTransform.rect.height || (Input.GetKey(KeyCode.UpArrow)))
        {
            velocity = transform.position + Vector3.up;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }
        else if (mouseController.mousePos.y <= ZERO || (Input.GetKey(KeyCode.DownArrow)))
        {
            velocity = transform.position - Vector3.up;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }
    }

    private void CameraZoom()
    {
        float zoomDir = Input.GetAxis("Mouse ScrollWheel");
        float currentSize = mainCamera.orthographicSize - zoomDir * zoomSpeed;

        mainCamera.orthographicSize = Mathf.Clamp(currentSize, zoomIn, zoomOut);
    }

    
}
