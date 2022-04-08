using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr instance;
    public AudioSource[] unitSound;
    public Unit[] units;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        if (unitSound == null)
        {
            return;
        }
        for (int i = 0; i < unitSound.Length; i++)
        {
            unitSound[i] = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i] = FindObjectOfType<Unit>();
        }
    }

    public void SFXSound(float volume)
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].theAudio.volume = volume;
        }
    }
}
