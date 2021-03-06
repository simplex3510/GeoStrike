using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


[DefaultExecutionOrder(202)]
public class AutoMoveCamera : MonoBehaviourPun
{
    [SerializeField] private CameraController mainCamera;

    private void Awake()
    {
        if (mainCamera == null) { mainCamera = GetComponent<CameraController>(); }

        if (GameMgr.isMaster)
        {
            PhotonNetwork.Instantiate("BuildZone", mainCamera.GetCameraStartPos("p1"), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("BuildZone", mainCamera.GetCameraStartPos("p2"), Quaternion.identity);
        }
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
            if (GameMgr.isMaster)
            {
                mainCamera.transform.position = mainCamera.GetCameraStartPos("p1");
            }
            else
            {
                mainCamera.transform.position = mainCamera.GetCameraStartPos("p2");
            }
        }
    }
}
