using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField]
    private float movespeed;

    [SerializeField]
    private Transform left;
    [SerializeField]
    private Transform right;
    [SerializeField]
    private Transform top;
    [SerializeField]
    private Transform bottom;


    [SerializeField]
    private Image minimapImage;
    [SerializeField]
    private Image minimapViewCamera;

    [SerializeField]
    private GameObject targetCamera;

    // Start is called before the first frame update
    void Start()
    {
        var inst = Instantiate(minimapImage.material);
        minimapImage.material = inst;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetCamera != null)
        {
            Vector2 mapArea = new Vector2(Vector3.Distance(left.position, right.position), Vector3.Distance(bottom.position, top.position));
            Vector2 charPos = new Vector2(Vector3.Distance(left.position, new Vector3(targetCamera.transform.position.x * Mathf.Abs(movespeed), 0f, 0f)),
                Vector3.Distance(bottom.position, new Vector3(0f, targetCamera.transform.position.y, 0f)));
            Vector2 normalPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);

            minimapViewCamera.rectTransform.anchoredPosition = new Vector2(minimapViewCamera.rectTransform.sizeDelta.x * normalPos.x, minimapViewCamera.rectTransform.sizeDelta.y * normalPos.y);
        }
    }
}
