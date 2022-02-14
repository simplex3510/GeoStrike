using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Detector : MonoBehaviourPun
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TetrominoCreater creater;

    private Ray ray;
    private RaycastHit hit;
    private RaycastHit2D hit2D;
    [SerializeField] LayerMask mask;

    [HideInInspector] public TetrominoTile tile;
    public static bool canBuild = true;
    public static bool canMove = true;

    private Vector2 battchModeMousePos;
    private RaycastHit2D unitTileHit2D;
    private static bool cancel = false;

    public GameObject tetrominoObj;
    public Tetromino tetromino;
    public Vector3 angle;

    [SerializeField] private GameObject clickedObject;      // 클릭한 Object 저장


    

    private void Awake()
    {
        if (cameraController == null) { cameraController = GameObject.FindObjectOfType<CameraController>(); }
        if (creater == null) { creater = GameObject.FindObjectOfType<TetrominoCreater>(); }
    }

    private void Update()
    {
        CheckBuildPreview();
        OnClickEvent();
    }


    private void OnClickEvent()
    {
        if (cameraController.mouseController.GetMode() == MouseController.EMouseMode.normal && Input.GetMouseButtonDown(0))
        {
            Vector2 pos = cameraController.mainCamera.ScreenToWorldPoint(cameraController.mouseController.mousePos);
            hit2D = Physics2D.Raycast(pos, Vector2.zero, 0f, mask);
            if (hit2D.collider != null)
            {
                // 유닛 정보 불러오기
                clickedObject = hit2D.collider.gameObject;
                if (clickedObject.CompareTag("unit"))
                {
                    // 정보창에 유닛 띄우기
                    Debug.Log("unit status : " + hit2D);
                    if (hit2D.collider.GetComponent<UnitState>().GetState() == UnitState.EUnitState.FSM_Standby)
                    {
                        // 배치모드
                        StartCoroutine(CBatchMode());
                    }
                }

                //if (hit2D.collider.CompareTag("Tetromino"))
                //{
                //    Debug.Log("UnitTile : " + hit2D);
                //}
            }
        }
    }

    private void CheckBuildPreview()
    {
        if (cameraController.mouseController.GetMode() == MouseController.EMouseMode.create)
        {
            ray = cameraController.mainCamera.ScreenPointToRay(cameraController.mouseController.mousePos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("TetrominoTile"))
                {
                    // Get the index of the tile at the selected position
                    tile = hit.transform.GetComponent<TetrominoTile>();

                    // Check tile befor Build tetromino
                    StartCoroutine(CCheckTile());
                }
            }
        }
    }

    private Vector3 GetTileSize()
    {
        float tileX = tile.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float tileY = tile.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector3 tilePointer = hit.transform.position - new Vector3(tileX, tileY);

        return tilePointer;
    }


    IEnumerator CCheckTile()
    {
        // Check tile
        //creater.CanBuildPreview(TetrominoPreview.instance.clickSlot.currentBlockShape, TetrominoPreview.instance.clickSlot.currentBlockRotation, tileIdx);

        // Build tetromino
        if (Input.GetMouseButton(0) && canBuild)
        {
            creater.BuildTetromino(tetrominoObj, tile.tileCoord, angle);
        }

        yield return null;
    }

    IEnumerator CBatchMode()
    {
        Debug.Log("set");
        while (cancel == Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            hit2D.transform.GetComponent<UnitTile>().SetColor();

            if (Input.GetMouseButton(0) && canMove)
            {
                // 유닛 이동  or 스왑
                Debug.Log("move");
            }
            Debug.Log("Cancel");
            yield return null;
        }
    }
}
