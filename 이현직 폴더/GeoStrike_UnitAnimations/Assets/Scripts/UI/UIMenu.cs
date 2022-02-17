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

    public Animation m_ani; // ���ϸ��̼� ���� ����Ʈ�� ������.

    void Awake()
    {
    }

    public void Click_UpDown()
    {
        if (m_isPlaying == true)
            return;

        m_isPlaying = true;
        StartCoroutine(IE_PlayUpDown()); // �������� m_isPlaying = false;
    }
    IEnumerator IE_PlayUpDown()
    {
        //=========================== ���ϸ��̼� ���� ó���Ҷ�.
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
