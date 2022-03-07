using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{
    public Image img;       // Ŭ���� ������Ʈ �̹���
    public Text textName;   // Ŭ���� ������Ʈ �̸�
    public Text textHP;     // Ŭ���� ������Ʈ ü��
    public Text textATK;    // Ŭ���� ������Ʈ ���ݷ�
    public Text textDEF;    // Ŭ���� ������Ʈ ����

    public void UnitStatusInfo(SpriteRenderer _spriterenderer, string _name, float _hp, float _atk, float _def)
    {
        img.sprite = _spriterenderer.sprite;
        textName.text = (_name);
        textHP.text = ("HP : " + _hp);
        textATK.text = ("Attack : " + _atk);
        textDEF.text = ("Defence : " + _def);
       
    }

    public void TetrominoStatusInfo()
    {

    }
}
