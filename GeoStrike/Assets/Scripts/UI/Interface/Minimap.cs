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

    public GameObject test;

    public void OnPointerClick(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();

        Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
        print(ray.origin);
        test.transform.position = ray.origin;
    }

    //public void OnClick()
    //{
    //    rectTransform = GetComponent<RectTransform>();
    //    //Debug.Log("맵 이미지 사이즈 : " + rectTransform.rect.size);
    //    // (237.3, 237.3)

    //    //이미지의 RectTransform 컴포넌트를 가져온다
    //    //Debug.Log("맵 이미지 사이즈 : " + rectTransform.offsetMin);

    //    Vector2 mousePos = Input.mousePosition;
    //    //Debug.Log("마우스를 클릭한 스크린 좌표 : " + mousePos);

    //    Vector2 clickedPos = mousePos - rectTransform.offsetMin;

    //    Vector2 ratioVec = mousePos / rectTransform.rect.size;

    //    Vector2 worldMapPosition;
    //    worldMapPosition.x = ratioVec.x * rectTransform.rect.size.x;
    //    worldMapPosition.y = ratioVec.y * rectTransform.rect.size.y;

    //    worldMapPosition = worldMapPosition / 2;

    //    Debug.Log("월드 좌표 : " + worldMapPosition);

    //    mainCamera.transform.position = worldMapPosition;
    //}
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    OnClick();
    //}
}
