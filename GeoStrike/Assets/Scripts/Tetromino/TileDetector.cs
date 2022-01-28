using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TetrominoCreater creater;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    [HideInInspector] public TetrominoTile tile;
    private int tileIdx;
    public static bool canBuild { get; set; }

    private void Awake()
    {
        mainCamera = Camera.main;
        canBuild = true;
    }

    private void Update()
    {
        CheckBuildPreview();
    }

    private Vector3 Get_TileSize()
    {
        float tileX = tile.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float tileY = tile.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector3 tilePointer = hit.transform.position - new Vector3(tileX, tileY);

        return tilePointer;
    }

    private void CheckBuildPreview()
    {
        if (cameraController.mouseController.emouseMode == MouseController.EMouseMode.create)
        {
            ray = mainCamera.ScreenPointToRay(cameraController.mouseController.mousePos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("TetrominoTile") && cameraController.mouseController.emouseMode == MouseController.EMouseMode.create)
                {
                    // Get the index of the tile at the selected position
                    tile = hit.transform.GetComponent<TetrominoTile>();
                    tileIdx = creater.tileContainer.tileList.IndexOf(tile);

                    // Check tile befor Build tetromino
                    StartCoroutine(CheckTile());
                }
            }
        }
    }

    IEnumerator CheckTile()
    {
        // Check tile
        creater.CanBuildPreview(TetrominoPreview.instance.clickSlot.currentBlockShape, TetrominoPreview.instance.clickSlot.currentBlockRotation, tileIdx);

        // Build tetromino
        if (Input.GetMouseButton(0) && canBuild)
        {
            creater.BuildTetromino(TetrominoPreview.instance.clickSlot.tetrominoPrefab, Get_TileSize(), TetrominoPreview.instance.clickSlot.currentBlockRotation, tileIdx);
        }

        yield return null;
    }
}
