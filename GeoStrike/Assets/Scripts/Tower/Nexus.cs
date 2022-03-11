using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(202)]
public class Nexus : Tower
{
    private void OnDisable()
    {
        // 파괴된 넥서스가 블루팀인지 레드팀인지 판별
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
