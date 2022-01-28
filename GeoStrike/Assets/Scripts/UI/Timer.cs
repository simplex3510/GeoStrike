using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // World timer
    [SerializeField] private Text worldTimeTXT;
    private float sec = 0f;
    private int min = 0;
    private readonly int MAX_SEC = 59;

    // Player timer
    [SerializeField] private Text playerTimeTXT;
    [SerializeField] private float waitingTime;
    public float playerATime = 0f;
    public float playerBTime = 0f;
    public bool isReady = false;

    private void Update()
    {
        WorldTimer();
        PlayerTimer();
    }

    private void PlayerTimer()
    {
        playerATime += Time.deltaTime;
        playerTimeTXT.text = string.Format("{0:D}s", (int)playerATime);

        if (playerATime >= waitingTime) 
        {
            isReady = true;
            playerATime = 0f;
        }
    }

    private void WorldTimer()
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
