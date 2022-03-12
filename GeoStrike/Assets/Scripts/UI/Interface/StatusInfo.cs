using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfo : MonoBehaviour
{
    public Image img;       // Ŭ���� ������Ʈ �̹���
    public Text textName;   // Ŭ���� ������Ʈ �̸�
    public Text textHP;     // Ŭ���� ������Ʈ ü��
    public Text textATK;    // Ŭ���� ������Ʈ ���ݷ�
    public Text textDEF;    // Ŭ���� ������Ʈ ����

    public void UnitStatusInfo(SpriteRenderer _spriteRenderer, string _name, float _hp, float _atk, float _def)
    {
        img.sprite = _spriteRenderer.sprite;
        textName.text = (_name);
        textHP.text = ("HP : " + _hp);
        textATK.text = ("Attack : " + _atk);
        textDEF.text = ("Defence : " + _def);
    }

    public void TetrominoStatusInfo(SpriteRenderer _spriteRenderer, Quaternion _quaternion, string _name)
    {
        img.sprite = _spriteRenderer.sprite;
        img.transform.rotation = _quaternion;
        textName.text = _name;
    }
}
