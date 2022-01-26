using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    [SerializeField] private CameraController m_cameraController;
    [SerializeField] private TetrominoCreater m_creater;

    private Camera m_mainCamera;
    private Ray m_ray;
    private RaycastHit m_hit;
    [HideInInspector] public TetrominoTile m_tile;
    private int m_tileIdx;
    public static bool m_canBuild { get; set; }

    private void Awake()
    {
        m_mainCamera = Camera.main;
        m_canBuild = true;
    }

    private void Update()
    {
        CheckBuildPreview();
    }

    private Vector3 Get_TileSize()
    {
        float tileX = m_tile.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float tileY = m_tile.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector3 tilePointer = m_hit.transform.position - new Vector3(tileX, tileY);

        return tilePointer;
    }

    private void CheckBuildPreview()
    {
        if (m_cameraController.m_mouseController.m_mouseMode == MouseController.E_MouseMode.create)
        {
            m_ray = m_mainCamera.ScreenPointToRay(m_cameraController.m_mouseController.m_mousePos);
            if (Physics.Raycast(m_ray, out m_hit, Mathf.Infinity))
            {
                if (m_hit.transform.CompareTag("BuildTile") && m_cameraController.m_mouseController.m_mouseMode == MouseController.E_MouseMode.create)
                {
                    // Get the index of the tile at the selected position
                    m_tile = m_hit.transform.GetComponent<TetrominoTile>();
                    m_tileIdx = m_creater.m_tileContainer.m_tileList.IndexOf(m_tile);

                    // Check tile befor Build tetromino
                    StartCoroutine(CheckTile());
                }
            }
        }
    }

    IEnumerator CheckTile()
    {
        // Check tile
        m_creater.CanBuildPreview(TetrominoPreview.instance.m_clickSlot.m_currentBlockShape, TetrominoPreview.instance.m_clickSlot.m_currentBlockRotation, m_tileIdx);

        // Build tetromino
        if (Input.GetMouseButton(0) && m_canBuild)
        {
            m_creater.BuildTetromino(TetrominoPreview.instance.m_clickSlot.m_tetrominoPrefab, Get_TileSize(), TetrominoPreview.instance.m_clickSlot.m_currentBlockRotation, m_tileIdx);
        }

        yield return null;
    }
}
