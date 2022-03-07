using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        float size;

        size = mainCamera.orthographicSize;

        print(mainCamera.orthographicSize);
        this.transform.localScale = new Vector3(size, size, size);
    }
}
