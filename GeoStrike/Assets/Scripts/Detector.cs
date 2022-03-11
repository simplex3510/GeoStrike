using System.Collections;
using UnityEngine;


public class Detector : MonoBehaviour
{
    [Header("< Component >")]
    [HideInInspector] private CameraController cameraController;
    [HideInInspector] private TetrominoCreater creater;
    [HideInInspector] private UnitTileContainer unitTileContainer;
    [HideInInspector] private UnitSelectEffect unitSelectEffect;

    // 마우스 위치, 클릭
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit2D hit2D;
    [SerializeField] private LayerMask mask;

    [HideInInspector] public TetrominoTile tile;    // 마우스 위치의 타일 정보
    public static bool canBuild = true;

    // 선택된 테트로미노 정보 변수
    [HideInInspector] public GameObject tetrominoObj;
    [HideInInspector] public Tetromino tetromino;
    [HideInInspector] public Vector3 angle;

    // 정보창 & 유닛 배치모드
    private GameObject clickedObject;      // 클릭한 Object 저장
    private Unit statusPanelUnit;           // 정보창에서 사용할 클릭한 Unit
    private Unit clickedUnit;              // 배치모드에서 사용할 클릭한 Unit 저장

    private void Awake()
    {
        if (cameraController == null) { cameraController = GameObject.FindObjectOfType<CameraController>(); }
        if (creater == null) { creater = GameObject.FindObjectOfType<TetrominoCreater>(); }
        if (unitTileContainer == null) { unitTileContainer = GameObject.FindObjectOfType<UnitTileContainer>(); }
        if (unitSelectEffect == null) { unitSelectEffect = GameObject.FindObjectOfType<UnitSelectEffect>(); }
    }

    private void Update()
    {
        ClickedObjectData();
        CheckBuildPreview();
    }

    private void LateUpdate()
    {
        StatusSlotUpdate();
    }

    // Status(정보창) 업데이트
    private void StatusSlotUpdate()
    {
        if (clickedObject != null)
        {
            if (clickedObject.CompareTag("Unit"))
            {
                statusPanelUnit = clickedObject.GetComponent<Unit>();
                GameMgr.instance.canvas.GetComponentInChildren<StatusPanel>().UnitStatusInfo(
                    statusPanelUnit.GetComponent<SpriteRenderer>(), statusPanelUnit.unitName, statusPanelUnit.currentHealth, statusPanelUnit.damage, statusPanelUnit.defense);
            }
            else if (clickedObject.CompareTag("Tetromino"))
            {
                // 구현중
                // 테트리스건물일 경우 띄워야 할 정보들
                // 이미지, 이름, 건물 완성도
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
                clickedUnit = clickedObject.GetComponent<Unit>();

                // 클릭한 유닛이 Idle (유닛타일에서 대기중) 일때 배치모드 실행
                if (clickedUnit.unitState == EUnitState.Idle && cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal)
                {
                    StartCoroutine(CBatchMode());
                }
            }
        }
    }

    // 테트로미노 빌드 이미지 위치설정 (테트로미노 타일에서만 이동가능)
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
        Transform temp;

        Debug.Log("batchMode");
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.batch;
        Cursor.lockState = CursorLockMode.Locked;
        while (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetMouseButtonDown(MouseController.CLICK_RIGHT) &&
               clickedUnit.unitState == EUnitState.Idle && GameMgr.instance.GetState() == EGameState.FSM_SpawnCount)
        {
            h = 0;
            v = 0;
            // 유닛 이동
            if (Input.GetKeyDown(KeyCode.W))
            {
                v = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                v = -1;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                h = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                h = +1;
            }
            int finalH = Mathf.Clamp(clickedUnit.column + h, 0, 7);
            int finalV = Mathf.Clamp(clickedUnit.row + v, 0, 7);

            // 이동 방향에 빈 타일일 경우
            if (!unitTileContainer.unitTransformArr[finalV, finalH])
            {
                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = null; // 현재 위치의 유닛 유무

                clickedUnit.row = finalV;
                clickedUnit.column = finalH;

                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = clickedUnit.transform; // 최종 위치의 유닛 유무
                clickedUnit.unitCreator.spawnPos += new Vector3(h, v, 0);   // Spawn 위치 지정
                clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // 유닛의 실제 위치 이동
            }
            // 이동 방향에 유닛이 있을경우 Swap
            //else
            //{
            //    unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = .transform; // 이동된 다른 유닛 transform

            //    clickedUnit.row = finalV;
            //    clickedUnit.column = finalH;

            //    unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = clickedUnit.transform;
            //    clickedUnit.unitCreator.spawnPos += new Vector3(h, v, 0);   // Spawn 위치 지정
            //    clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // 유닛의 실제 위치 이동
            //}
            
            unitSelectEffect.transform.position = Camera.main.WorldToScreenPoint(clickedUnit.transform.position);
            yield return null;
        }

        // 배치모드 끝날시
        clickedUnit = null;
        unitSelectEffect.transform.position = unitSelectEffect.originPos;
        Cursor.lockState = CursorLockMode.None;
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.normal;
        Debug.Log("Cancel");
    }
}
