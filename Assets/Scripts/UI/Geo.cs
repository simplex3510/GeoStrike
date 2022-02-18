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

    public static readonly float GEO_INCREASE = 2;
    public static readonly float GEO_SQUARE = 3;
    public static readonly float GEO_BOUNUS = 10;

    private void Awake()
    {
        geoImage.sprite = getSprite;
    }

    private void Update()
    {
        if (GameMgr.instance.GetState() != EGameState.FSM_Standby)
        {
            GeoTxT();
            DeltaGeo(Time.deltaTime / GEO_INCREASE);
        }
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
