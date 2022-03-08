using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Detector : MonoBehaviour
{
    [Header("< Component >")]
    [HideInInspector] private CameraController cameraController;
    [HideInInspector] private TetrominoCreater creater;
    [HideInInspector] private UnitTileContainer unitTileContainer;

    // ���콺 ��ġ, Ŭ��
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit2D hit2D;
    [SerializeField] LayerMask mask;

    [HideInInspector] public TetrominoTile tile;    // ���콺 ��ġ�� Ÿ�� ����
    public static bool canBuild = true;

    // ���õ� ��Ʈ�ι̳� ���� ����
    [HideInInspector] public GameObject tetrominoObj;
    [HideInInspector] public Tetromino tetromino;
    [HideInInspector] public Vector3 angle;

    // ����â & ���� ��ġ��� - ������
    [SerializeField] private GameObject clickedObject;      // Ŭ���� Object ����
    [SerializeField] private Unit clickedUnit;              // Ŭ���� Unit ����


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
        // Status(����â) ������Ʈ
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
                // ������
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

                // Ŭ���� obj�� �����̰� Idle (����Ÿ�Ͽ��� �����) �϶� ��ġ��� ����
                StartCoroutine(CBatchMode());
            }
        }
    }

    // ��Ʈ�ι̳� �̸����� ��ġ���� (��Ʈ�ι̳� Ÿ�Ͽ����� �̵�����)
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
                // ������ Ÿ�� �� �ٲٱ� �Ķ�, ����
                //hit2D.transform.GetComponent<UnitTile>().SetColor();

                // ���� �̵�
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
