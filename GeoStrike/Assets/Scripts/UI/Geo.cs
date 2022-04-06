using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geo : MonoBehaviour
{
    [SerializeField] private Image geoImage;
    [SerializeField] private Sprite getSprite;
    [SerializeField] private Text geoTXT;
    
    private static float currentGeo = 100;
    public static float CurrentGeo { get { return currentGeo; } }

    public static readonly float GEO_SQUARE = 50;

    private void Awake()
    {
        geoImage.sprite = getSprite;
    }

    private void Update()
    {
        if (GameMgr.instance.GetState() != EGameState.Standby)
        {
            GeoTxT();
            DeltaGeo(Time.deltaTime * 5);

            // 개발자모드
            if (Input.GetKeyDown(KeyCode.F3))
            {
                DeltaGeo(300);
            }
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
