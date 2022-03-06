using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Detector : MonoBehaviour
{
    [Header("< Component >")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TetrominoCreater creater;

    // ���콺 ��ġ, Ŭ��
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit2D hit2D;
    [SerializeField] LayerMask mask;

    [HideInInspector] public TetrominoTile tile;    // ���콺 ��ġ�� Ÿ�� ����
    public static bool canBuild = true;
    public static bool canMove = true;

    // ���õ� ��Ʈ�ι̳� ���� ����
    public GameObject tetrominoObj;
    public Tetromino tetromino;
    public Vector3 angle;

    // ����â & ���� ��ġ��� - ������
    [SerializeField] private GameObject clickedObject;      // Ŭ���� Object ����
    [SerializeField] private Unit clickedUnit;              // Ŭ���� Unit ����

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

    // ����â ����
    private void OnClickEvent()
    {
        if (cameraController.mouseController.eMouseMode == MouseController.EMouseMode.normal && Input.GetMouseButtonDown(MouseController.CLICK_LEFT))
        {
            Vector2 pos = cameraController.mainCamera.ScreenToWorldPoint(cameraController.mouseController.mousePos);
            hit2D = Physics2D.Raycast(pos, Vector2.zero, 0f, mask);
            if (hit2D.collider != null)
            {
                // Ŭ���� Obj ���� �ҷ�����
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
                // ���� �̵�  or ����
                Debug.Log("move");
            }
            Debug.Log("Cancel");
            yield return null;
        }
    }
}
