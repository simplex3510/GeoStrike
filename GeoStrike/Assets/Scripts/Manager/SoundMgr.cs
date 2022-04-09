using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr instance;
    public Unit[] units;
    public Grenade[] grenades;

    public Slider effect;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        units = FindObjectsOfType<Unit>();
        grenades = FindObjectsOfType<Grenade>();
        SFXSound(effect.value);
    }

    public void SFXSound(float volume)
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].theAudio.volume = volume;
        }
        for (int i = 0; i < grenades.Length; i++)
        {
            grenades[i].theAudio.volume = volume;
        }
    }
}
