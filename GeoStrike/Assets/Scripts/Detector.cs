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

    // Status(정보창) 업데이트
    IEnumerator EStatusSlotUpdate()
    {
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
        yield return null;
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

                // Status(정보창) 업데이트
                StartCoroutine(EStatusSlotUpdate());

                // 클릭한 유닛이 Idle (유닛타일에서 대기중) 일때 배치모드 실행
                if (clickedUnit.GetUnitState() == EUnitState.Idle && cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal)
                {
                    StartCoroutine(CBatchMode());
                }
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
        int h = 0;
        int v = 0;
        Debug.Log("batchMode");
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.build;
        while (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetMouseButtonDown(MouseController.CLICK_RIGHT) && clickedUnit.GetUnitState() == EUnitState.Idle)
        {
            h = 0;
            v = 0;

            // 유닛 이동
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                v = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                v = -1;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                h = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                h = +1;
            }
            int finalH = Mathf.Clamp(clickedUnit.column + h, 0, 7);
            int finalV = Mathf.Clamp(clickedUnit.row + v, 0, 7);

            // 이동 가능한지 체크하기
            if (!unitTileContainer.checkUnitArr[finalV, finalH])
            {
                unitTileContainer.checkUnitArr[clickedUnit.row, clickedUnit.column] = false; // 현재 위치

                clickedUnit.row = finalV;
                clickedUnit.column = finalH;

                unitTileContainer.checkUnitArr[clickedUnit.row, clickedUnit.column] = true; // 최종 위치
                clickedUnit.unitCreator.spawnPos += new Vector3(h, v, 0);   // Spawn 위치 지정
                clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // 유닛의 실제 위치 이동
            }

            yield return null;
        }
        clickedUnit = null;
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.normal;
        Debug.Log("Cancel");
    }


    // 질문 2 : 유닛이 위치한 좌표 아래의 타일에 접근방법 (유닛 중복배치 제한)
    // 작업 3 : 타일 배열크기에 맞게 유닛 이동 제한 시키기
    //IEnumerator CBatchMode()
    //{
    //    Debug.Log("batchMode");
    //    cameraController.mouseController.eMouseMode = MouseController.EMouseMode.build;
    //    while (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetMouseButtonDown(MouseController.CLICK_RIGHT) && clickedUnit.GetUnitState() == EUnitState.Idle)
    //    {
    //        // 유닛 이동
    //        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // GetAxisRow 대체 가능
    //        {
    //            Debug.Log("move w");
    //            unitTileContainer.checkUnitArr[clickedUnit.row, clickedUnit.column] = false;
    //            clickedUnit.unitCreator.spawnPos += Vector3.up;
    //            unitTileContainer.checkUnitArr[clickedUnit.row + 1, clickedUnit.column] = true;
    //            clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos;
    //        }
    //        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
    //        {
    //            Debug.Log("move s");
    //            unitTileContainer.checkUnitArr[clickedUnit.row, clickedUnit.column] = false;
    //            clickedUnit.unitCreator.spawnPos += Vector3.down;
    //            unitTileContainer.checkUnitArr[clickedUnit.row - 1, clickedUnit.column] = true;
    //            clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos;
    //        }
    //        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
    //        {
    //            Debug.Log("move a");
    //            unitTileContainer.checkUnitArr[clickedUnit.row, clickedUnit.column] = false;
    //            clickedUnit.unitCreator.spawnPos += Vector3.left;
    //            unitTileContainer.checkUnitArr[clickedUnit.row, clickedUnit.column - 1] = true;
    //            clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos;
    //        }
    //        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
    //        {
    //            Debug.Log("move d");
    //            unitTileContainer.checkUnitArr[clickedUnit.row, clickedUnit.column] = false;
    //            clickedUnit.unitCreator.spawnPos += Vector3.right;
    //            unitTileContainer.checkUnitArr[clickedUnit.row + 1, clickedUnit.column + 1] = true;
    //            clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos;
    //        }
    //        yield return null;
    //    }
    //    clickedUnit = null;
    //    cameraController.mouseController.eMouseMode = MouseController.EMouseMode.normal;
    //    Debug.Log("Cancel");
    //}
}
