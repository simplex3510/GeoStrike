using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class KeySlotInfo : MonoBehaviour
{
    // Obj Ŭ�� - ��Ʈ�ι̳��ϰ�� - ��ȭ��ư - ������ - creator.unit���� (�ǹ��� ���� ���ְ�ȭ�� �޸���)- �ش� �ǹ��� ��� ���ְ�ȭ
    // obj Ŭ�� - �����ϰ�� - ��Ʋ�ʵ忡 ���� ��� - ������ ���� None
    //                      - Idle���� - ���� - ���� ���� ������
    //                                 - ����� - ����� ���� ������

    public Detector detector;
    public Button[] buttonArr = new Button[3];

    private void Awake()
    {
        if (detector == null) { detector = GameObject.FindObjectOfType<Detector>(); }
        buttonArr = GetComponentsInChildren<Button>();
    }

    [SerializeField]
    public void OnButtonBuffAndDebuff(string _buff)
    {
        Enum.TryParse<EBuffandDebuff>(_buff, out EBuffandDebuff result);
        detector.clickedUnit.GetComponentInChildren<Buff>().CurrentBuff = result;
        Debug.Log("Choice : " + result);
    }
}
