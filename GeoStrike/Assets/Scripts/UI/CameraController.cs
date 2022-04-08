
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
    public AutoMoveCamera autoMoveCamera;
    [SerializeField] private RectTransform rectTransform;     // Resolution 1920 x 1080

    // Camera movement
    [Header("< Camera controller >")]
    [SerializeField] private float cameraSpeed;
    private readonly int ZERO = 0;
    
    // Camera Zoom
    [SerializeField] private float zoomSpeed = 0f;
    [SerializeField] private float zoomIn = 0f;
    [SerializeField] private float zoomOut = 0f;

    // Start Pos
    private Vector3 p1Pos = new Vector3(-32.5f, 1f, -16f);
    private Vector3 p2Pos = new Vector3( 32.5f, 1f, -16f);

    // Auto Move BuildZone
    public bool onZone { get; set; }

    private void Awake()
    {
        if (mainCamera == null)    { mainCamera = GetComponent<Camera>(); }
        if (mouseController == null)    { mouseController = GetComponent<MouseController>(); }
        if (autoMoveCamera  == null)    { autoMoveCamera  = GetComponent<AutoMoveCamera>(); }

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
            mainCamera.transform.position = p1Pos;
        }
        else
        {
            mainCamera.transform.position = p2Pos;
        }
    }

    private void CameraMovement()
    {
        if (mouseController.eMouseMode != MouseController.EMouseMode.normal) { return; }

        Vector3 velocity;
        int h = 0;
        int v = 0;

        // Camera move horizotnal
        if ( mouseController.mousePos.x >= rectTransform.rect.width - 10 || (Input.GetKey(KeyCode.D)))
        {
            h = 1;
        }
        else if (mouseController.mousePos.x <= ZERO + 10 || (Input.GetKey(KeyCode.A)))
        {
            h = -1;
        }

        // Camera move verticals
        if (mouseController.mousePos.y >= rectTransform.rect.height - 10 || (Input.GetKey(KeyCode.W)))
        {
            v = 1;
        }
        else if (mouseController.mousePos.y <= ZERO + 10 || (Input.GetKey(KeyCode.S)))
        {
            v = -1;
        }

        velocity = transform.position + new Vector3(h, 0, v);
        velocity.x = Mathf.Clamp(velocity.x, -36, 36);
        velocity.z = Mathf.Clamp(velocity.z, -20, 17);
        
        transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
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
    
    public Vector3 GetCameraStartPos(string _pNum)
    {
        if (_pNum == "p1")
        {
            return p1Pos;
        }
        else if (_pNum == "p2")
        {
            return p2Pos;
        }
        else
        {
            return Vector3.zero;
        }

    }
}
