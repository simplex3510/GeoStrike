using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geo : MonoBehaviour
{
    [SerializeField] private Image geoImage;
    [SerializeField] private Sprite getSprite;
    [SerializeField] private Text geoTXT;
    private float currentGeo = 0;

    private void Awake()
    {
        geoImage.sprite = getSprite;
    }

    private void Update()
    {
        GeoTxT();
        DeltaGeo(Time.deltaTime / 2);
    }

    private void GeoTxT()
    {
        geoTXT.text = string.Format(": {0}", (int)currentGeo);
    }

    public void DeltaGeo(float _delta)
    {
        currentGeo += _delta;
    }
}
