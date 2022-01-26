using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // World timer
    [SerializeField] private Text m_worldTimeTXT;
    private float m_sec = 0f;
    private int m_min = 0;
    private readonly int MAX_SEC = 59;

    // Player timer
    [SerializeField] private Text m_playerTimeTXT;
    [SerializeField] private float m_waitingTime;
    public float m_playerATime = 0f;
    public float m_playerBTime = 0f;
    public bool m_isReady = false;

    private void Update()
    {
        WorldTimer();
        PlayerTimer();
    }

    private void PlayerTimer()
    {
        m_playerATime += Time.deltaTime;
        m_playerTimeTXT.text = string.Format("{0:D}s", (int)m_playerATime);

        if (m_playerATime >= m_waitingTime) 
        {
            m_isReady = true;
            m_playerATime = 0f;
        }
    }

    private void WorldTimer()
    {
        m_sec += Time.deltaTime;
        m_worldTimeTXT.text = string.Format("{0:D2} : {1:D2}", m_min, (int)m_sec);

        if((int)m_sec > MAX_SEC)
        {
            m_sec = 0;
            m_min++;
        }
    }
}
