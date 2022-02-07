using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public float durationTime = 0.5f;
    public bool isOpen = false; // on/off
    public bool isPlaying = false;

    public Animation ani; // ���ϸ��̼� ����Ʈ�� ������.

    void Awake()
    {
    }

    public void Click_OpenClose()
    {
        if (isPlaying == true)
            return;

        isPlaying = true;
        StartCoroutine(IE_PlayOpenClose()); // �������� isPlaying = false;
    }
    IEnumerator IE_PlayOpenClose()
    {
        //=========================== ���ϸ��̼� ���� ó���Ҷ�.
        if (!isOpen)
        {
            ani.Play("UI_Menu_Open");
            isOpen = true;
        }
        else
        {
            ani.Play("UI_Menu_Close");
            isOpen = false;
        }
        yield return new WaitForSeconds(durationTime);

        isPlaying = false;
    }
}
