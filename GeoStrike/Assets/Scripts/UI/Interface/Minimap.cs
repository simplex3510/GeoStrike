using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera minimapCamera;

    RectTransform rectTransform;
    Vector3 mousePos;

    public GameObject test;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            rectTransform = GetComponent<RectTransform>();

            mousePos = Input.mousePosition;
            mousePos = minimapCamera.ScreenToWorldPoint(mousePos);

            mainCamera.transform.position = new Vector3(mousePos.x, 10, mousePos.z);
        }
    }
}
