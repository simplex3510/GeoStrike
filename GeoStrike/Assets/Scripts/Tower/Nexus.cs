using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(202)]
public class Nexus : Tower
{
    private void OnDisable()
    {
        // �ı��� �ؼ����� ��������� ���������� �Ǻ�
        if (photonView.IsMine && GameMgr.isMaster || !photonView.IsMine && !GameMgr.isMaster)
        {
            GameMgr.blueNexus = false;
        }
        else
        {
            GameMgr.redNexus = false;
        }

        GameMgr.instance.SetState(EGameState.FSM_GameEnd);
    }
}
