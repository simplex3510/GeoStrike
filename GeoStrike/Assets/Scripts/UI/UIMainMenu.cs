using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public float durationTime = 0.5f;
    public bool isOpen = false; // on/off
    public bool isPlaying = false;

    public Animation ani; // 에니메이션 리스트를 가진다.

    public Slider[] soundSlider;
    public Text[] soundValue;

    void Awake()
    {
    }

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            soundValue[i].text = soundSlider[i].value.ToString();
        }
    }

    public void Click_OpenClose()
    {
        if (isPlaying == true)
            return;

        isPlaying = true;
        StartCoroutine(IE_PlayOpenClose()); // 마지막에 isPlaying = false;
    }
    IEnumerator IE_PlayOpenClose()
    {
        //=========================== 에니메이션 으로 처리할때.
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
