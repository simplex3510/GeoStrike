using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Minimap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera minimapCamera;

    RectTransform rectTransform;
    Vector3 mousePos;

    public GameObject test;

    public void OnPointerClick(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();

        mousePos = Input.mousePosition;
        mousePos = minimapCamera.ScreenToWorldPoint(mousePos);

        mainCamera.transform.position = new Vector3(mousePos.x, 10, mousePos.z);
    }
}
