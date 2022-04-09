using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr instance;
    public Unit[] units;


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
    }

    public void SFXSound(float volume)
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].theAudio.volume = volume;
        }
    }
}
