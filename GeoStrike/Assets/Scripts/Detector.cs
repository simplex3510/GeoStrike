using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Detector : MonoBehaviour
{
    [Header("< Component >")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TetrominoCreater creater;

    // 마우스 위치, 클릭
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit2D hit2D;
    [SerializeField] LayerMask mask;

    [HideInInspector] public TetrominoTile tile;    // 마우스 위치의 타일 정보
    public static bool canBuild = true;
    public static bool canMove = true;

    // 선택된 테트로미노 정보 변수
    public GameObject tetrominoObj;
    public Tetromino tetromino;
    public Vector3 angle;

    // 정보창 & 유닛 배치모드 - 보류중
    [SerializeField] private GameObject clickedObject;      // 클릭한 Object 저장
    [SerializeField] private Unit clickedUnit;              // 클릭한 Unit 저장

    private Vector2 battchModeMousePos;
    private RaycastHit2D unitTileHit2D;
    private static bool cancel = false;


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

    private void LateUpdate()
    {
        // Status(정보창) 업데이트
        if (clickedObject != null)
        {
            if (clickedObject.CompareTag("Unit"))
            {
                clickedUnit = clickedObject.GetComponent<Unit>();
                GameMgr.instance.canvas.GetComponentInChildren<StatusPanel>()
                    .UnitStatusInfo(clickedUnit.GetComponent<SpriteRenderer>(), clickedUnit.unitName, clickedUnit.currentHealth, clickedUnit.damage, clickedUnit.defense);
            }
            else if (clickedObject.CompareTag("Tetromino"))
            {
                // 구현중
            }
        }
    }

    // 상태창 띄우기
    private void OnClickEvent()
    {
        if (cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal && Input.GetMouseButtonDown(MouseController.CLICK_LEFT))
        {
            Vector2 pos = cameraController.mainCamera.ScreenToWorldPoint(cameraController.mouseController.mousePos);
            hit2D = Physics2D.Raycast(pos, Vector2.zero, 0f, mask);
            if (hit2D.collider != null)
            {
                // 클릭한 Obj 정보 불러오기
                clickedObject = hit2D.collider.gameObject;
            }
        }
    }

    private void CheckBuildPreview()
    {
        if (cameraController.mouseController.eMouseMode == MouseController.EMouseMode.build)
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

    IEnumerator CCheckTile()
    {
        // Build tetromino
        if (Input.GetMouseButton(MouseController.CLICK_LEFT))
        {
            creater.BuildTetromino(tetrominoObj, hit.transform.position, tile.tileCoord, tetromino.GetCoordinate(), angle);
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
