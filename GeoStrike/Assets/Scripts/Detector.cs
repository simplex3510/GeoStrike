using System.Collections;
using UnityEngine;


public class Detector : MonoBehaviour
{
    [Header("< Component >")]
    [HideInInspector] private CameraController cameraController;
    [HideInInspector] private TetrominoCreater creater;
    [HideInInspector] private UnitTileContainer unitTileContainer;
    [HideInInspector] private UnitSelectEffect unitSelectEffect;

    // ���콺 ��ġ, Ŭ��
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit2D hit2D;
    [SerializeField] private LayerMask mask;

    [HideInInspector] public TetrominoTile tile;    // ���콺 ��ġ�� Ÿ�� ����
    public static bool canBuild = true;

    // ���õ� ��Ʈ�ι̳� ���� ����
    [HideInInspector] public GameObject tetrominoObj;
    [HideInInspector] public Tetromino tetromino;
    [HideInInspector] public Vector3 angle;

    // ����â & ���� ��ġ���
    private GameObject clickedObject;      // Ŭ���� Object ����
    private Unit statusPanelUnit;           // ����â���� ����� Ŭ���� Unit
    private Unit clickedUnit;              // ��ġ��忡�� ����� Ŭ���� Unit ����

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

    // Status(����â) ������Ʈ
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
                // ������
                // ��Ʈ�����ǹ��� ��� ����� �� ������
                // �̹���, �̸�, �ǹ� �ϼ���
            }
        }
    }


    // Ŭ���� ������Ʈ Data ��������
    private void ClickedObjectData()
    {
        if (cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal && Input.GetMouseButtonDown(MouseController.CLICK_LEFT))
        {
            Vector2 pos = cameraController.mainCamera.ScreenToWorldPoint(cameraController.mouseController.mousePos);
            hit2D = Physics2D.Raycast(pos, Vector2.zero, 0f, mask);
            if (hit2D.collider != null)
            {
                // Ŭ���� Obj ���� �ҷ�����
                clickedObject = hit2D.collider.gameObject;
                clickedUnit = clickedObject.GetComponent<Unit>();

                // Ŭ���� ������ Idle (����Ÿ�Ͽ��� �����) �϶� ��ġ��� ����
                if (clickedUnit.unitState == EUnitState.Idle && cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal)
                {
                    StartCoroutine(CBatchMode());
                }
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
            if (!unitTileContainer.unitTransformArr[finalV, finalH])
            {
                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = null; // ���� ��ġ�� ���� ����

                clickedUnit.row = finalV;
                clickedUnit.column = finalH;

                unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = clickedUnit.transform; // ���� ��ġ�� ���� ����
                clickedUnit.unitCreator.spawnPos += new Vector3(h, v, 0);   // Spawn ��ġ ����
                clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // ������ ���� ��ġ �̵�
            }
            // �̵� ���⿡ ������ ������� Swap
            //else
            //{
            //    unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = .transform; // �̵��� �ٸ� ���� transform

            //    clickedUnit.row = finalV;
            //    clickedUnit.column = finalH;

            //    unitTileContainer.unitTransformArr[clickedUnit.row, clickedUnit.column] = clickedUnit.transform;
            //    clickedUnit.unitCreator.spawnPos += new Vector3(h, v, 0);   // Spawn ��ġ ����
            //    clickedUnit.transform.position = clickedUnit.unitCreator.spawnPos; // ������ ���� ��ġ �̵�
            //}
            
            unitSelectEffect.transform.position = Camera.main.WorldToScreenPoint(clickedUnit.transform.position);
            yield return null;
        }

        // ��ġ��� ������
        clickedUnit = null;
        unitSelectEffect.transform.position = unitSelectEffect.originPos;
        Cursor.lockState = CursorLockMode.None;
        cameraController.mouseController.eMouseMode = MouseController.EMouseMode.normal;
        Debug.Log("Cancel");
    }
}
