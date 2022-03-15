
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
    [HideInInspector] public Vector3 p1Pos = new Vector3(-32.5f, 1f, -16f);
    [HideInInspector] public Vector3 p2Pos = new Vector3( 32.5f, 1f, -16f);

    // Auto Move BuildZone
    public bool onZone { get; set; }

    private void Awake()
    {
        if (mainCamera      == null)    { mainCamera      = GetComponent<Camera>(); }
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
        Vector3 velocity;
        int h = 0;
        int v = 0;

        // Camera move horizotnal
        if (/* mouseController.mousePos.x >= rectTransform.rect.width || */(Input.GetKey(KeyCode.RightArrow)))
        {
            h = 1;
        }
        else if (/*mouseController.mousePos.x <= ZERO || */(Input.GetKey(KeyCode.LeftArrow)))
        {
            h = -1;
        }

        // Camera move verticals
        if (/*mouseController.mousePos.y >= rectTransform.rect.height || */(Input.GetKey(KeyCode.UpArrow)))
        {
            v = 1;
        }
        else if (/*mouseController.mousePos.y <= ZERO || */(Input.GetKey(KeyCode.DownArrow)))
        {
            v = -1;
        }

        velocity = transform.position + new Vector3(h, 0, v);
        //velocity.x = Mathf.Clamp(velocity.x, -36, 36);
        //velocity.y = Mathf.Clamp(velocity.y, -30, 7);
        
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

     
}
