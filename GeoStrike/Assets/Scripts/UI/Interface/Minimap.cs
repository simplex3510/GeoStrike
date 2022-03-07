using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Minimap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera minimapCamera;

    int zoomSpeed = 1;

    //private void Start()
    //{
    //    RaycastHit hit;
    //    Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        Transform objectHit = hit.transform;


    //    }
    //}

    private void Update()
    {
        Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
        float zoomDistance = zoomSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        mainCamera.transform.Translate(ray.direction * zoomDistance, Space.World);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
        //float zoomDistance = zoomSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        //mainCamera.transform.Translate(ray.direction * zoomDistance, Space.World);

        RaycastHit hit;
        Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
        }
    }
}
