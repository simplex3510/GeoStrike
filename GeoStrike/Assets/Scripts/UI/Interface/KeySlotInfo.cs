using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeySlotInfo : MonoBehaviour
{
    // Obj Ŭ�� - ��Ʈ�ι̳��ϰ�� - ��ȭ��ư - ������ - creator.unit���� (�ǹ��� ���� ���ְ�ȭ�� �޸���)- �ش� �ǹ��� ��� ���ְ�ȭ
    // obj Ŭ�� - �����ϰ�� - ��Ʋ�ʵ忡 ���� ��� - ������ ���� None
    //                      - Idle���� - ���� - ���� ���� ������
    //                                 - ����� - ����� ���� ������

    public Detector detector;

    private void Awake()
    {
        if (detector == null) { detector = GameObject.FindObjectOfType<Detector>(); }
    }

    [SerializeField]
    public void OnButtonBuff(string _buff)
    {
        Enum.TryParse<EBuffandDebuff>(_buff, out EBuffandDebuff result);
        detector.clickedUnit.GetComponentInChildren<Buff>().CurrentBuff = result;
        Debug.Log("Choice : " + result);
    }

    //public void OnButtonAttack()
    //{
    //    detector.clickedUnit.GetComponentInChildren<Buff>().CurrentBuff =
    //    Debug.Log("choice");
    //}

    //public void OnButtonDefence()
    //{
    //    //detector.clickedUnit.GetComponentInChildren<Buff>().ChoiceBuffDefence();
    //    Debug.Log("choice");
    //}

    //public void OnButtonHaste()
    //{
    //    //detector.clickedUnit.GetComponentInChildren<Buff>().ChoiceBuffHaste();
    //    Debug.Log("choice");
    //}
}
