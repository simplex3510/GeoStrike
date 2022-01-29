using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geo : MonoBehaviour
{
    [SerializeField] Timer timer;

    [SerializeField] private Image geoImage;
    [SerializeField] private Sprite getSprite;
    [SerializeField] private Text geoTXT;
    public float currentGeo = 0;

    private void Awake()
    {
        geoImage.sprite = getSprite;
    }

    private void Update()
    {
        GeoTxT();
    }

    private void GeoTxT()
    {
        geoTXT.text = string.Format(": {0}", currentGeo);
    }

    public void DeltaGeo(float _delta)
    {
        currentGeo += _delta;
    }
}
