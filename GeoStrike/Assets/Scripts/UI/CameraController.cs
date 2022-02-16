
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraController : MonoBehaviourPunCallbacks
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

    // Start player camera position;
    private Transform startPosP1;
    private Transform startPosP2;

    private void Awake()
    {
        if (mainCamera == null) { mainCamera = GetComponent<Camera>(); }
        if (mouseController == null) { mouseController = GetComponent<MouseController>(); }

        if (startPosP1 == null) { startPosP1 = GameObject.FindGameObjectWithTag("StartPosP1").GetComponent<Transform>(); }
        if (startPosP2 == null) { startPosP2 = GameObject.FindGameObjectWithTag("StartPosP2").GetComponent<Transform>(); }
        
        InitStartPos();
    }

    private void Update()
    {
        CameraMovement();
        CameraZoom();
    }

    private void InitStartPos()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            mainCamera.transform.position = startPosP1.position;
        }
        else
        {
            mainCamera.transform.position = startPosP2.position;
        }
    } 

    private void CameraMovement()
    {
        Vector3 velocity;
    
        // Camera move horizotnal
        if (mouseController.mousePos.x >= rectTransform.rect.width || (Input.GetKey(KeyCode.RightArrow)))
        {
            velocity = transform.position + Vector3.right;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }
        else if (mouseController.mousePos.x <= ZERO || (Input.GetKey(KeyCode.LeftArrow)))
        {
            velocity = transform.position - Vector3.right;
            transform.position = Vector3.Lerp(transform.position, velocity, cameraSpeed);
        }

        // Camera move verticals
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
