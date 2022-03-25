using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum ESpecialSkills
{
    ElectricShock = 0,  // ������
    Repair,             // ��        
    Snare               // �ӹ�
}


public class SpecialSkills : MonoBehaviour
{
    public List<SpecialSkills> skillList = new List<SpecialSkills>();
    public Image skillImage;

    public void TryOnSkill(string _skillName)
    {
        // ���Կ� ��ϵ� ��ų�� ������
        // ��ų�� ������ ���콺��ġ�� �׷�����
        // �ѹ��� Ŭ���ϸ� OnSkill�� �ȴ�.

        Enum.TryParse<ESpecialSkills>(_skillName, out ESpecialSkills result);
        Debug.Log("Skill : " + result);
    }
}
