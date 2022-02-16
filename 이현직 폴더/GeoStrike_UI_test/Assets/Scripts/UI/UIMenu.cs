using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public float m_speed = 50f;
    public float m_durationTime = 0.5f;
    public bool m_isOpen = false; // on/off
    public bool m_isPlaying = false;

    public Animation m_ani; // 에니메이션 츨립 리스트를 가진다.

    void Awake()
    {
    }

    public void Click_UpDown()
    {
        if (m_isPlaying == true)
            return;

        m_isPlaying = true;
        StartCoroutine(IE_PlayUpDown()); // 마지막에 m_isPlaying = false;
    }
    IEnumerator IE_PlayUpDown()
    {
        //=========================== 에니메이션 으로 처리할때.
        if (!m_isOpen)
        {
            m_ani.Play("UIMenuOpen");
            m_isOpen = true;
        }
        else
        {
            m_ani.Play("UIMenuClose");
            m_isOpen = false;
        }
        yield return new WaitForSeconds(m_durationTime);

        m_isPlaying = false;
    }
}
