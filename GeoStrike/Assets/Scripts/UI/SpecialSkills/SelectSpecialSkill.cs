using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSpecialSkill : MonoBehaviour
{
    public Button[] buttonArr = new Button[3];
    public SpecialSkills specialSkills;
    int[] rand = new int[3];

    private void OnEnable()
    {
        buttonArr = GetComponentsInChildren<Button>();
        SetSkillOnButton();
    }

    private void SetSkillOnButton()
    {
        CreateRandomNumber();
        for (int idx = 0; idx < buttonArr.Length; idx++)
        {
            SpecialSkills specialSkill = buttonArr[idx].GetComponent<SkillDataOnButton>().specialSkill;
            specialSkill = specialSkills.skillList[rand[idx]];
        }
    }

    private void CreateRandomNumber()
    {
        for (int idx = 0; idx < rand.Length; idx++)
        {
            rand[idx] = -1;
        }

        // �ߺ� üũ
        while (true)
        {
            int temp = Random.Range(0, specialSkills.skillList.Count);
            for (int idx = 0; idx < rand.Length; idx++)
            {
                if (rand[idx] == temp) // �̹� �������� ���������
                {
                    break;
                }
                else if (rand[idx] == -1)
                {
                    rand[idx] = temp;
                    break;
                }
            }

            // ��� �迭�� �������� �ԷµǸ� While�� ����������
            if ( rand[rand.Length - 1] != -1 )
            {
                break;
            }
        }

        for (int idx = 0; idx < rand.Length; idx++)
        {
            Debug.Log("Rand : " + rand[idx]);
        }
        
    }
}
