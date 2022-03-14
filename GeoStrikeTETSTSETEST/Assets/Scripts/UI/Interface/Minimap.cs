using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Minimap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera minimapCamera;

    RectTransform rectTransform;
    Vector2 mousePos;

    public GameObject test;

    public void OnPointerClick(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();

        //Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log(hit.point);
        //    mainCamera.transform.position = new Vector3(hit.point.x, hit.point.y, -10);

        //    Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 0.3f);
        //}

        mousePos = Input.mousePosition;
        mousePos = minimapCamera.ScreenToWorldPoint(mousePos);

        mainCamera.transform.position = new Vector3(mousePos.x, mousePos.y, -10);
    }
}
