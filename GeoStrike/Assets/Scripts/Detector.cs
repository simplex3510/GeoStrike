using System.Collections;
using UnityEngine;


public class Detector : MonoBehaviour
{
    [Header("< Component >")]
    [HideInInspector] private CameraController cameraController;
    [HideInInspector] private TetrominoCreater creater;
    [HideInInspector] private UnitTileContainer unitTileContainer;
    [HideInInspector] private UnitSelectEffect unitSelectEffect;
    [HideInInspector] private StatusPanel statusPanel;
    [HideInInspector] private KeySlotPanel keySlotPanel;

    // 마우스 위치, 클릭
    private Ray ray;
    private RaycastHit hit;
    [SerializeField] private LayerMask mask;

    [HideInInspector] public TetrominoTile tile;    // 마우스 위치의 타일 정보
    public static bool canBuild = true;

    // 선택된 테트로미노 정보 변수
    [HideInInspector] public GameObject tetrominoObj;
    [HideInInspector] public Tetromino tetromino;
    [HideInInspector] public Vector3 angle;

    // 정보창 & 유닛 배치모드
    public GameObject clickedObject;      // 클릭한 Object 저장
    public Unit clickedUnit;              // 배치모드에서 사용할 클릭한 Unit 저장

    private void Awake()
    {
        if (cameraController == null) { cameraController = GameObject.FindObjectOfType<CameraController>(); }
        if (creater == null) { creater = GameObject.FindObjectOfType<TetrominoCreater>(); }
        if (unitTileContainer == null) { unitTileContainer = GameObject.FindObjectOfType<UnitTileContainer>(); }
        if (unitSelectEffect == null) { unitSelectEffect = GameObject.FindObjectOfType<UnitSelectEffect>(); }
        if (statusPanel == null) { statusPanel = GameObject.FindObjectOfType<StatusPanel>(); }
        if (keySlotPanel == null) { keySlotPanel = GameObject.FindObjectOfType<KeySlotPanel>(); }
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
            if (clickedObject.CompareTag("Tetromino"))
            {
                // 구현중
                // 테트리스건물일 경우 띄워야 할 정보들
                // 이미지, 이름, 건물 완성도
                Tetromino tetromino = clickedObject.GetComponent<Tetromino>();
                statusPanel.statusInfoArr[0].TetrominoStatusInfo(tetromino.GetComponent<SpriteRenderer>(), tetromino.quaternion, tetromino.shapeName);
            }
            else if (clickedObject.CompareTag("Unit"))
            {
                Unit unit = clickedObject.GetComponent<Unit>();
                statusPanel.statusInfoArr[1].UnitStatusInfo(unit.GetComponentInChildren<SpriteRenderer>(), unit.unitName, unit.currentHealth, unit.damage, unit.defense);
            }
            else if (clickedObject.CompareTag("Tower"))
            {
                Debug.Log("Clicked Tower");
            }
        }
    }

    // 클릭한 오브젝트 Data 가져오기
    private void ClickedObjectData()
    {
        if (cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal && Input.GetMouseButtonDown(MouseController.CLICK_LEFT))
        {
            ray = cameraController.mainCamera.ScreenPointToRay(cameraController.mouseController.mousePos);
            Physics.Raycast(ray, out hit, Mathf.Infinity, mask);

            if (hit.collider != null)
            {
                // 클릭한 Obj 정보 불러오기
                clickedObject = hit.collider.gameObject;
                clickedUnit = clickedObject.GetComponent<Unit>();

                // 클릭한 obj 따라 Status, KeySlot창 띄우기
                WhichObjInterface();

                // 클릭한 유닛이 Idle (유닛타일에서 대기중) 일때 배치모드 실행
                if (clickedObject.CompareTag("Unit") && clickedUnit.unitState == EUnitState.Idle && cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal)
                {
                    StartCoroutine(CBatchMode());
                }
            }
        }
    }

    private void WhichObjInterface()
    {
        if (clickedObject.CompareTag("Tetromino"))
        {
            statusPanel.SetActiveFalseAll();
            statusPanel.statusInfoArr[0].gameObject.SetActive(true);

            keySlotPanel.SetActiveFalseAll();
            keySlotPanel.keySlotArr[0].gameObject.SetActive(true);
        }
        else if (clickedObject.CompareTag("Unit"))
        {
            statusPanel.SetActiveFalseAll();
            statusPanel.statusInfoArr[1].gameObject.SetActive(true);

            keySlotPanel.SetActiveFalseAll();

            // 버퍼와 디버퍼 기술 선택창
            if (clickedUnit.initStatus.unitIndex == EUnitIndex.Buffer)
            {
                Debug.Log("Buffer`s Enhance");
                keySlotPanel.keySlotArr[1].gameObject.SetActive(true);
            }
            else if (clickedUnit.initStatus.unitIndex == EUnitIndex.Debuffer)
            {
                Debug.Log("Debuffer`s Enhance");
                keySlotPanel.keySlotArr[1].gameObject.SetActive(true);
            }
            
        }
        else if (clickedObject.CompareTag("Tower"))
        {
            statusPanel.SetActiveFalseAll();
            statusPanel.statusInfoArr[2].gameObject.SetActive(true);

            keySlotPanel.SetActiveFalseAll();
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
        //Cursor.lockState = CursorLockMode.Locked;
        while (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetMouseButtonDown(MouseController.CLICK_RIGHT) &&
               clickedUnit != null && clickedUnit.unitState == EUnitState.Idle && GameMgr.instance.GetState() == EGameState.SpawnCount)
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
            // 필요 연산 : 1.TransformArr, 2.Unit.row & Unit.column, 3. unit.unitCreator.spawnPos, 4. unit.transform.position
            if (new Vector2(v, h) != Vector2.zero && unitTileContainer.unitTransformArr[finalV, finalH] == null)
            {
                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = null; // 현재 위치의 유닛 유무

                clickedUnit.row = finalV;
                clickedUnit.column = finalH;

                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = clickedUnit.transform; // 최종 위치의 유닛 유무
                clickedUnit.unitCreator.spawnPos += new Vector3(h, 0, v);   // Spawn 위치 지정
                clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // 유닛의 실제 위치 이동
            }
            //이동 방향에 유닛이 있을경우 Swap
            else if (new Vector2(v, h) != Vector2.zero && unitTileContainer.unitTransformArr[finalV, finalH] != null &&
                     unitTileContainer.unitTransformArr[finalV, finalH].GetComponent<Unit>() != clickedUnit)
            {
                // temp = Unit A : 클릭한 유닛을 temp에 넣기
                temp = unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column];

                // unit B : A가 이동할 자리에있는 유닛B를 A로 이동
                Transform unitB = unitTileContainer.unitTransformArr[finalV, finalH];
                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = unitB;
                unitB.GetComponent<Unit>().row = temp.GetComponent<Unit>().row;
                unitB.GetComponent<Unit>().column = temp.GetComponent<Unit>().column;
                unitB.GetComponent<Unit>().unitCreator.spawnPos = temp.GetComponent<Unit>().unitCreator.spawnPos;
                unitB.transform.position = temp.GetComponent<Unit>().unitCreator.spawnPos;
                
                // Unit A : temp를 이동할 자리로 이동
                clickedUnit.row = finalV;
                clickedUnit.column = finalH;

                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = temp;
                clickedUnit.unitCreator.spawnPos += new Vector3(h, 0, v);   // Spawn 위치 지정
                clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // 유닛의 실제 위치 이동
            }

            unitSelectEffect.transform.position = Camera.main.WorldToScreenPoint(clickedUnit.transform.position);
            yield return null;
        }

        // 배치모드 끝날시
        clickedUnit = null;
        unitSelectEffect.transform.position = unitSelectEffect.originPos;
        //Cursor.lockState = CursorLockMode.None;
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.normal;
        Debug.Log("Cancel");
    }
}
