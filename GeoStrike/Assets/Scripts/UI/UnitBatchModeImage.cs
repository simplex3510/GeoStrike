using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(202)]
public class UnitBatchModeImage : MonoBehaviour
{
    public Image image;
    public Vector3 originPos;

    private void Awake()
    {
        if (image == null) { image = GetComponent<Image>(); }
        originPos = this.transform.position;

        if (GameMgr.isMaster)
        {
            image.sprite = Resources.Load<Sprite>("UI/Sprites/Blue_UnitSelect");
        }
        else
        {
            image.sprite = Resources.Load<Sprite>("UI/Sprites/Red_UnitSelect");
        }
    }
}
