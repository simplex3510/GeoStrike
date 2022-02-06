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
            Debug.Log(mainCamera.onZone);
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("BuildZone"))
        {
            mainCamera.onZone = false;
            Debug.Log(mainCamera.onZone);
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
