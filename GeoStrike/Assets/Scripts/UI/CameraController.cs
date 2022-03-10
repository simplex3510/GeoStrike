
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DefaultExecutionOrder(202)]
public class CameraController : MonoBehaviour
{
    [Header("< Componenet >")]
    public Camera mainCamera;
    public MouseController mouseController;
    [SerializeField] private RectTransform rectTransform;     // Resolution 1920 x 1080

    // Camera movement
    [Header("< Camera controller >")]
    [SerializeField] private float cameraSpeed;
    private readonly int ZERO = 0;
    
    // Camera Zoom
    [SerializeField] private float zoomSpeed = 0f;
    [SerializeField] private float zoomIn = 0f;
    [SerializeField] private float zoomOut = 0f;


    // Auto Move BuildZone
    public bool onZone { get; set; }

    private void Awake()
    {
        if (mainCamera == null) { mainCamera = GetComponent<Camera>(); }
        if (mouseController == null) { mouseController = GetComponent<MouseController>(); }
 
        InitStartPos();
    }

    private void Update()
    {
        CameraMovement();
        CameraZoom();
    }

    private void InitStartPos()
    {
        if (GameMgr.isMaster)
        {
            mainCamera.transform.position = new Vector3 (-31.7f, -24f, -10f);
        }
        else
        {
            mainCamera.transform.position = new Vector3(31.7f, -24f, -10f);
        }
    }

    private void CameraMovement()
    {
        Vector3 velocity;
    
        // Camera move horizotnal
        if (/* mouseController.mousePos.x >= rectTransform.rect.width || */(Input.GetKey(KeyCode.RightArrow)))
        {
            velocity = transform.position + Vector3.right;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }
        else if (/*mouseController.mousePos.x <= ZERO || */(Input.GetKey(KeyCode.LeftArrow)))
        {
            velocity = transform.position - Vector3.right;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }

        // Camera move verticals
        if (/*mouseController.mousePos.y >= rectTransform.rect.height || */(Input.GetKey(KeyCode.UpArrow)))
        {
            velocity = transform.position + Vector3.up;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }
        else if (/*mouseController.mousePos.y <= ZERO || */(Input.GetKey(KeyCode.DownArrow)))
        {
            velocity = transform.position - Vector3.up;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }
    }

    private void CameraZoom()
    {
        if (mouseController.eMouseMode == MouseController.EMouseMode.normal)
        {
            float zoomDir = Input.GetAxis("Mouse ScrollWheel");
            float currentSize = mainCamera.orthographicSize - zoomDir * zoomSpeed;

            mainCamera.orthographicSize = Mathf.Clamp(currentSize, zoomIn, zoomOut);
        }
        else
        {
            mainCamera.orthographicSize = zoomIn;
        }
    }

     
}
