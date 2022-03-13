using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(202)]
public class Nexus : Tower
{
    private void OnDisable()
    {
        // �ı��� �ؼ����� ��������� ���������� �Ǻ�
        if (GameMgr.isMaster && photonView.IsMine || !GameMgr.isMaster && !photonView.IsMine)
        {
            GameMgr.blueNexus = false;
        }
        else 
        {
            GameMgr.redNexus = false;
        }

        GameMgr.instance.SetState(EGameState.GameEnd);
    }
}
