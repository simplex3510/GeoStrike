using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("< World Time >")]
    [SerializeField] private Text worldTimeTXT;
    public float sec = 0f;
    private int min = 0;
    private readonly int MAX_SEC = 59;

    [Header("< Battle Timer >")]
    [SerializeField] private Text battleTimeTXT;
    [SerializeField] private float battleTime;
    public float battleTimer = 0f;
    public bool isReady = false;

    private void Update()
    {
        WorldTime();
        BattleTime();
    }

    private void BattleTime()
    {
        battleTimer += Time.deltaTime;
        battleTimeTXT.text = string.Format("{0:D}s", (int)battleTimer);

        if (battleTimer >= battleTime) 
        {
            isReady = true;
            battleTimer = 0f;
        }
    }

    private void WorldTime()
    {
        sec += Time.deltaTime;
        worldTimeTXT.text = string.Format("{0:D2} : {1:D2}", min, (int)sec);

        if((int)sec > MAX_SEC)
        {
            sec = 0;
            min++;
        }
    }
}
