using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveCamera : MonoBehaviour
{
    [SerializeField] private CameraController mainCamera;

    private void Awake()
    {
        if (mainCamera == null) { mainCamera = GetComponent<CameraController>(); }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("BuildZone"))
        {
            mainCamera.onZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("BuildZone"))
        {
            mainCamera.onZone = false;
        }
    }

    public void MoveToBuildZone()
    {
        if (!mainCamera.onZone)
        {
            mainCamera.transform.position = new Vector3(-32f, -24f, -10f);
        }
    }
}
