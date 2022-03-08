using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Detector : MonoBehaviour
{
    [Header("< Component >")]
    [HideInInspector] private CameraController cameraController;
    [HideInInspector] private TetrominoCreater creater;
    [HideInInspector] private UnitTileContainer unitTileContainer;

    // 마우스 위치, 클릭
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit2D hit2D;
    [SerializeField] LayerMask mask;

    [HideInInspector] public TetrominoTile tile;    // 마우스 위치의 타일 정보
    public static bool canBuild = true;

    // 선택된 테트로미노 정보 변수
    [HideInInspector] public GameObject tetrominoObj;
    [HideInInspector] public Tetromino tetromino;
    [HideInInspector] public Vector3 angle;

    // 정보창 & 유닛 배치모드 - 보류중
    [SerializeField] private GameObject clickedObject;      // 클릭한 Object 저장
    [SerializeField] private Unit clickedUnit;              // 클릭한 Unit 저장


    private void Awake()
    {
        if (cameraController == null) { cameraController = GameObject.FindObjectOfType<CameraController>(); }
        if (creater == null) { creater = GameObject.FindObjectOfType<TetrominoCreater>(); }
        if (unitTileContainer == null) { unitTileContainer = GameObject.FindObjectOfType<UnitTileContainer>(); }
    }

    private void Update()
    {
        ClickedObjectData();
        CheckBuildPreview();
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

    // 클릭한 오브젝트 Data 가져오기
    private void ClickedObjectData()
    {
        if (cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal && Input.GetMouseButtonDown(MouseController.CLICK_LEFT))
        {
            Vector2 pos = cameraController.mainCamera.ScreenToWorldPoint(cameraController.mouseController.mousePos);
            hit2D = Physics2D.Raycast(pos, Vector2.zero, 0f, mask);
            if (hit2D.collider != null)
            {
                // 클릭한 Obj 정보 불러오기
                clickedObject = hit2D.collider.gameObject;

                // 클릭한 obj가 유닛이고 Idle (유닛타일에서 대기중) 일때 배치모드 실행
                StartCoroutine(CBatchMode());
            }
        }
    }

    // 테트로미노 미리보기 위치설정 (테트로미노 타일에서만 이동가능)
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
                    StartCoroutine(CBuildTetromino());
                }
            }
        }
    }

    IEnumerator CBuildTetromino()
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
        if (clickedObject.CompareTag("Unit") || clickedUnit.GetUnitState() == EUnitState.Idle)
        {
            Debug.Log("batchMode");
            while (!Input.GetKeyDown(KeyCode.Escape) || !Input.GetMouseButtonDown(1))
            {
                // 유닛의 타일 색 바꾸기 파랑, 빨강
                //hit2D.transform.GetComponent<UnitTile>().SetColor();

                // 유닛 이동
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Debug.Log("move w");
                    clickedUnit.transform.position += Vector3.up; 
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Debug.Log("move s");
                    clickedUnit.transform.position += Vector3.down;
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Debug.Log("move a");
                    clickedUnit.transform.position += Vector3.left;
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Debug.Log("move d");
                    clickedUnit.transform.position += Vector3.right;
                }
                yield return null;
            }
            Debug.Log("Cancel");
        }
    }
}
