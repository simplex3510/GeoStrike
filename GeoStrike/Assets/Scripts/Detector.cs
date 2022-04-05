using System.Collections;
using UnityEngine;


public class Detector : MonoBehaviour
{
    // Components
    [HideInInspector] public StatusPanel statusPanel;
    [HideInInspector] private CameraController cameraController;
    [HideInInspector] private TetrominoCreater creater;
    [HideInInspector] private UnitTileContainer unitTileContainer;
    [HideInInspector] private UnitBatchModeImage unitBatchModeImage;
    [HideInInspector] private SelectImage selectImage;
    [HideInInspector] private KeySlotPanel keySlotPanel;

    // ���콺 ��ġ, Ŭ��
    private Ray ray;
    private RaycastHit hit;
    [SerializeField] private LayerMask mask;

    [HideInInspector] public TetrominoTile tile;    // ���콺 ��ġ�� Ÿ�� ����
    public static bool canBuild = true;

    // ���õ� ��Ʈ�ι̳� ���� ����
    [HideInInspector] public GameObject tetrominoObj;
    [HideInInspector] public Tetromino tetromino;
    [HideInInspector] public Vector3 angle;

    // ����â & ���� ��ġ���
    public GameObject clickedObject;      // Ŭ���� Object ����
    public Unit clickedUnit;              // ��ġ��忡�� ����� Ŭ���� Unit ����
    Coroutine currentCoroutine;           // ��ġ��� ���� �������� �ڷ�ƾ
    Coroutine testCoroutine;
    

    private void Awake()
    {
        if (cameraController == null) { cameraController = GameObject.FindObjectOfType<CameraController>(); }
        if (creater == null) { creater = GameObject.FindObjectOfType<TetrominoCreater>(); }
        if (unitTileContainer == null) { unitTileContainer = GameObject.FindObjectOfType<UnitTileContainer>(); }
        if (unitBatchModeImage == null) { unitBatchModeImage = GameObject.FindObjectOfType<UnitBatchModeImage>(); }
        if (selectImage == null) { selectImage = GameObject.FindObjectOfType<SelectImage>(); }
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

    // Status(����â) ������Ʈ
    private void StatusSlotUpdate()
    {
        if (clickedObject != null)
        {
            selectImage.transform.position = Camera.main.WorldToScreenPoint(clickedObject.transform.position);

            if (clickedObject.CompareTag("Tetromino"))
            {
                Tetromino tetromino = clickedObject.GetComponent<Tetromino>();
                statusPanel.statusInfoArr[0].TetrominoStatusInfo(tetromino.GetComponent<SpriteRenderer>(), tetromino.quaternion, tetromino.shapeName);
            }
            else if (clickedObject.CompareTag("Unit"))
            {
                Unit unit = clickedObject.GetComponent<Unit>();
                statusPanel.statusInfoArr[1].UnitStatusInfo(unit.GetComponentInChildren<SpriteRenderer>(), unit.initStatus.unitName, unit.currentHealth, unit.damage, unit.defense);
                if (unit == null)
                {
                    statusPanel.SetActiveFalseAll();
                }
            }
            else if (clickedObject.CompareTag("Tower"))
            {
                // �ͷ� �̸� �������ֱ�, �̱��� : ���ݷ�
                Tower tower = clickedObject.GetComponent<Tower>();
                statusPanel.statusInfoArr[2].TowerStatusInfo(tower.GetComponent<SpriteRenderer>(), tower.initStatus.name, tower.Health, tower.Defense);
            }
        }
        else
        {
            selectImage.transform.position = selectImage.originPos;
        }
    }

    public void InitInterface()
    {
        clickedObject = null;
        clickedUnit = null;
        statusPanel.SetActiveFalseAll();
        selectImage.transform.position = selectImage.originPos;
    }

    // Ŭ���� ������Ʈ Data ��������
    private void ClickedObjectData()
    {
        if (cameraController.mouseController.eMouseMode != MouseController.EMouseMode.build && Input.GetMouseButtonDown(MouseController.CLICK_LEFT))
        {
            ray = cameraController.mainCamera.ScreenPointToRay(cameraController.mouseController.mousePos);
            Physics.Raycast(ray, out hit, Mathf.Infinity, mask);

            if (hit.collider != null)
            {
                // Ŭ���� Obj ���� �ҷ�����
                clickedObject = hit.collider.gameObject;
                clickedUnit = clickedObject.GetComponent<Unit>();

                // Ŭ���� obj ���� Status, KeySlotâ ����
                if (testCoroutine != null)
                {
                    StopCoroutine(testCoroutine);
                    OnClickListenersClear();
                }
                testCoroutine = StartCoroutine(WhichObjInterface());

                // Ŭ���� ������ Idle (����Ÿ�Ͽ��� �����) �϶� ��ġ��� ����
                if (clickedObject.CompareTag("Unit") && clickedUnit.unitState == EUnitState.Idle && clickedUnit.photonView.IsMine)
                {
                    // ù �ڷ�ƾ ����ó��
                    if (currentCoroutine != null)
                    {
                        // �ڷ�ƾ �ߺ� ���� ����
                        StopCoroutine(currentCoroutine);
                    }
                    // ���� �������� �ڷ�ƾ �ּ�
                    currentCoroutine = StartCoroutine(CBatchMode());
                }
            }
            else
            {
                InitInterface();
            }
        }
    }


    // ��Ʈ�ι̳� ���� �̹��� ��ġ���� (��Ʈ�ι̳� Ÿ�Ͽ����� �̵�����)
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

    private void OnClickListenersClear()
    {
        for (int idx = 0; idx < keySlotPanel.keySlotArr[0].buttonArr.Length; idx++)
        {
            keySlotPanel.keySlotArr[0].buttonArr[idx].onClick.RemoveAllListeners();
        }
    }
    IEnumerator WhichObjInterface()
    {
        if (clickedObject.CompareTag("Tetromino"))
        {
            statusPanel.SetActiveFalseAll();
            statusPanel.statusInfoArr[0].gameObject.SetActive(true);

            keySlotPanel.SetActiveFalseAll();
            keySlotPanel.keySlotArr[0].gameObject.SetActive(true);

            if (GameMgr.isMaster)
            {
                keySlotPanel.keySlotArr[0].buttonArr[0].onClick.AddListener(clickedObject.GetComponent<UnitCreator>().unitP1.OnEnforceDamage);
                keySlotPanel.keySlotArr[0].buttonArr[1].onClick.AddListener(clickedObject.GetComponent<UnitCreator>().unitP1.OnEnforceDefense);
                keySlotPanel.keySlotArr[0].buttonArr[2].onClick.AddListener(clickedObject.GetComponent<UnitCreator>().unitP1.OnEnforceHealth);
            }
            else
            {
                keySlotPanel.keySlotArr[0].buttonArr[0].onClick.AddListener(clickedObject.GetComponent<UnitCreator>().unitP2.OnEnforceDamage);
                keySlotPanel.keySlotArr[0].buttonArr[1].onClick.AddListener(clickedObject.GetComponent<UnitCreator>().unitP2.OnEnforceDefense);
                keySlotPanel.keySlotArr[0].buttonArr[2].onClick.AddListener(clickedObject.GetComponent<UnitCreator>().unitP2.OnEnforceHealth);
            }
        }
        else if (clickedObject.CompareTag("Unit"))
        {
            statusPanel.SetActiveFalseAll();
            statusPanel.statusInfoArr[1].gameObject.SetActive(true);

            keySlotPanel.SetActiveFalseAll();

            // ���ۿ� ����� ��� ����â
            if (clickedUnit.initStatus.unitIndex == EUnitIndex.Buffer && clickedUnit.unitState == EUnitState.Idle)
            {
                keySlotPanel.keySlotArr[1].gameObject.SetActive(true);
            }
            else if (clickedUnit.initStatus.unitIndex == EUnitIndex.Debuffer && clickedUnit.unitState == EUnitState.Idle)
            {
                keySlotPanel.keySlotArr[1].gameObject.SetActive(true);
            }

        }
        else if (clickedObject.CompareTag("Tower"))
        {
            statusPanel.SetActiveFalseAll();
            statusPanel.statusInfoArr[2].gameObject.SetActive(true);

            keySlotPanel.SetActiveFalseAll();
        }
        yield return null;
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
        // ī�޶� ��ġ �����ʵ�� �̵�
        if (GameMgr.isMaster)
        {
            cameraController.transform.position = cameraController.GetCameraStartPos("p1") + new Vector3(15.5f, 0f, 0f);
        }
        else
        {
            cameraController.transform.position = cameraController.GetCameraStartPos("p2") + new Vector3(15.5f, 0f, 0f);
        }

        int h = 0;
        int v = 0;
        Transform temp;

        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.batch;

        // ��ġ��� ������� : ESC, ��Ŭ��, �ð��� ���� ���� �̵�
        while (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetMouseButtonDown(MouseController.CLICK_RIGHT) && clickedUnit != null &&
               clickedUnit.gameObject.layer == (int)EPlayer.Ally && clickedUnit.unitState == EUnitState.Idle && GameMgr.instance.GetState() == EGameState.SpawnCount)
        {
            h = 0;
            v = 0;
            // ���� �̵�
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

            // �̵� ���⿡ �� Ÿ���� ���
            // �ʿ� ���� : 1.TransformArr, 2.Unit.row & Unit.column, 3. unit.unitCreator.spawnPos, 4. unit.transform.position
            if (new Vector2(v, h) != Vector2.zero && unitTileContainer.unitTransformArr[finalV, finalH] == null)
            {
                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = null; // ���� ��ġ�� ���� ����

                clickedUnit.row = finalV;
                clickedUnit.column = finalH;

                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = clickedUnit.transform; // ���� ��ġ�� ���� ����
                clickedUnit.unitCreator.spawnPos += new Vector3(h, 0, v);   // Spawn ��ġ ����
                clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // ������ ���� ��ġ �̵�
            }
            //�̵� ���⿡ ������ ������� Swap
            else if (new Vector2(v, h) != Vector2.zero && unitTileContainer.unitTransformArr[finalV, finalH] != null &&
                     unitTileContainer.unitTransformArr[finalV, finalH].GetComponent<Unit>() != clickedUnit)
            {
                // temp = Unit A : Ŭ���� ������ temp�� �ֱ�
                temp = unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column];

                // unit B : A�� �̵��� �ڸ����ִ� ����B�� A�� �̵�
                Transform unitB = unitTileContainer.unitTransformArr[finalV, finalH];
                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = unitB;
                unitB.GetComponent<Unit>().row = temp.GetComponent<Unit>().row;
                unitB.GetComponent<Unit>().column = temp.GetComponent<Unit>().column;
                unitB.GetComponent<Unit>().unitCreator.spawnPos = temp.GetComponent<Unit>().unitCreator.spawnPos;
                unitB.transform.position = temp.GetComponent<Unit>().unitCreator.spawnPos;
                
                // Unit A : temp�� �̵��� �ڸ��� �̵�
                clickedUnit.row = finalV;
                clickedUnit.column = finalH;

                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = temp;
                clickedUnit.unitCreator.spawnPos += new Vector3(h, 0, v);   // Spawn ��ġ ����
                clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // ������ ���� ��ġ �̵�
            }

            unitBatchModeImage.transform.position = Camera.main.WorldToScreenPoint(clickedUnit.transform.position);
            yield return null;
        }

        // ��ġ��� ������
        clickedUnit = null;
        unitBatchModeImage.transform.position = unitBatchModeImage.originPos;
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.normal;
    }
}
