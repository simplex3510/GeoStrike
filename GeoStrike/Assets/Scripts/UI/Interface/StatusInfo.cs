using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfo : MonoBehaviour
{
    public Image img;       // 클릭한 오브젝트 이미지
    public Text textName;   // 클릭한 오브젝트 이름
    public Text textHP;     // 클릭한 오브젝트 체력
    public Text textATK;    // 클릭한 오브젝트 공격력
    public Text textDEF;    // 클릭한 오브젝트 방어력

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
