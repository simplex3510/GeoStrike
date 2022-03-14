using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geo : MonoBehaviour
{
    [SerializeField] private Image geoImage;
    [SerializeField] private Sprite getSprite;
    [SerializeField] private Text geoTXT;
    
    private static float currentGeo = 0;
    public static float CurrentGeo { get { return currentGeo; } }

    public static readonly float GEO_SQUARE = 3;
    public static readonly float GEO_BOUNUS = 10;

    private void Awake()
    {
        geoImage.sprite = getSprite;
    }

    private void Update()
    {
        if (GameMgr.instance.GetState() != EGameState.Standby)
        {
            GeoTxT();
            DeltaGeo(Time.deltaTime);
        }
    }

    private void GeoTxT()
    {
        geoTXT.text = string.Format(": {0}", (int)currentGeo);
    }

    public static void DeltaGeo(float _delta)
    {
        currentGeo += _delta;
    }
}
