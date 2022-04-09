using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr instance;
    public List<Unit> units;
    public List<Grenade> grenades;

    public Slider effect;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        SFXSound(effect.value);
    }

    public void SFXSound(float volume)
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].theAudio.volume = volume;
        }
        for (int i = 0; i < grenades.Count; i++)
        {
            grenades[i].theAudio.volume = volume;
        }
    }
}
