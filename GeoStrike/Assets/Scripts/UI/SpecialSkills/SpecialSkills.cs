using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum ESpecialSkills
{
    ElectricShock = 0,  // 데미지
    Repair,             // 힐        
    Snare               // 속박
}


public class SpecialSkills : MonoBehaviour
{
    public List<SpecialSkills> skillList = new List<SpecialSkills>();

    public void TryOnSkill(string _skillName)
    {
        // 슬롯에 등록된 스킬을 누른다
        // 스킬의 범위가 마우스위치에 그려진다
        // 한번더 클릭하면 OnSkill이 된다.

        Enum.TryParse<ESpecialSkills>(_skillName, out ESpecialSkills result);
        Debug.Log("Skill : " + result);
    }
}
