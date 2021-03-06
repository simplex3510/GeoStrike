using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class KeySlotInfo : MonoBehaviour
{
    // Obj 클릭 - 테트로미노일경우 - 강화버튼 - 누르기 - creator.unit참조 (건물에 따른 유닛강화가 달리함)- 해당 건물의 모든 유닛강화
    // obj 클릭 - 유닛일경우 - 배틀필드에 있을 경우 - 누를거 없음 None
    //                      - Idle상태 - 버퍼 - 버프 선택 누르기
    //                                 - 디버퍼 - 디버프 선택 누르기

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
