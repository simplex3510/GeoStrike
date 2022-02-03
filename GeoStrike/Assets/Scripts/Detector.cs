using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TetrominoCreater creater;

    private Ray ray;
    private RaycastHit hit;
    private RaycastHit2D hit2D;
    [SerializeField] LayerMask mask;

    [HideInInspector] public TetrominoTile tile;
    private int tileIdx;
    public static bool canBuild = true;

    private void Update()
    {
        CheckBuildPreview();
        OnClickEvent();
    }


    private void OnClickEvent()
    {
        if (cameraController.mouseController.emouseMode == MouseController.EMouseMode.normal && Input.GetMouseButtonDown(0))
        {
            Vector2 pos = cameraController.mainCamera.ScreenToWorldPoint(cameraController.mouseController.mousePos);
            hit2D = Physics2D.Raycast(pos, Vector2.zero, 0f, mask);
            if (hit2D.collider != null)
            {
                // 유닛 정보 불러오기
                if (hit2D.collider.CompareTag("Unit"))
                {
                    Debug.Log("Unit Status : " + hit2D);
                }

                if (hit2D.collider.CompareTag("UnitTile"))
                {
                    Debug.Log("UnitTile : " + hit2D);
                }
            }
        }
    }

    private void CheckBuildPreview()
    {
        if (cameraController.mouseController.emouseMode == MouseController.EMouseMode.create)
        {
            ray = cameraController.mainCamera.ScreenPointToRay(cameraController.mouseController.mousePos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("TetrominoTile"))
                {
                    // Get the index of the tile at the selected position
                    tile = hit.transform.GetComponent<TetrominoTile>();
                    tileIdx = creater.tileContainer.TetrominotileList.IndexOf(tile);

                    // Check tile befor Build tetromino
                    StartCoroutine(CheckTile());
                }
            }
        }
    }

    private Vector3 GetTileSize()
    {
        float tileX = tile.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float tileY = tile.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector3 tilePointer = hit.transform.position - new Vector3(tileX, tileY);

        return tilePointer;
    }


    IEnumerator CheckTile()
    {
        // Check tile
        creater.CanBuildPreview(TetrominoPreview.instance.clickSlot.currentBlockShape, TetrominoPreview.instance.clickSlot.currentBlockRotation, tileIdx);

        // Build tetromino
        if (Input.GetMouseButton(0) && canBuild)
        {
            creater.BuildTetromino(TetrominoPreview.instance.clickSlot.tetrominoPrefab, GetTileSize(), TetrominoPreview.instance.clickSlot.currentBlockRotation, tileIdx);
        }

        yield return null;
    }
}
