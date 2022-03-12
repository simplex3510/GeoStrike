using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


[DefaultExecutionOrder(202)]
public class GameState : MonoBehaviourPun
{
    [SerializeField] private StandbyCount standbyCount;
    
    public static readonly int GAMESTATE_STANDBYTIME = 5;
    public static readonly int GAMESTATE_COUNT = 1;

    private void Start()
    {
        StartCoroutine(FSM());
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(GameMgr.instance.GetState().ToString());
        }
    }

    // °ÔÀÓ ½ÃÀÛ Àü ÁØºñ ½Ã°£ (Ä«¿îÆ® ´Ù¿î)
    IEnumerator FSM_Standby()
    {
        for(int count = GAMESTATE_STANDBYTIME; count > 0; count--)
        {
            standbyCount.stanbyCountTXT.text = string.Format("{0}", count);
            yield return new WaitForSeconds(GAMESTATE_COUNT);
        }
        standbyCount.gameObject.SetActive(false);

        Debug.Log("Game Start");
        GameMgr.instance.SetState(EGameState.FSM_SpawnCount);
    }

    // BattleTimer 0s ~ Xs »çÀÌ
    IEnumerator FSM_SpawnCount()
    {
        while (GameMgr.instance.GetState() == EGameState.FSM_SpawnCount)
        {
            yield return null;
        }
    }

    // 0s°¡ µÇ´Â ¼ø°£ (SpawnTime)
    IEnumerator FSM_Battle()
    {
        while(GameMgr.instance.GetState() == EGameState.FSM_Battle)
        {
            yield return null;
        }
    }

    IEnumerator FSM_GameEnd()
    {
        // 1.ºí·çÆÀ ³Ø¼­½ºÆÄ±«
        // ºí·çÆÀ ÆÐ¹è
        // ·¹µåÆÀ ½Â¸®

        // 2.·¹µåÆÀ ³Ø¼­½º ÆÄ±«
        // ºí·çÆÀ ½Â¸®
        // ·¹µåÆÀ ÆÐ¹è

        if (GameMgr.blueNexus == false)
        {
            if (GameMgr.isMaster)
            {
                PhotonNetwork.LoadLevel("DefeateScene");
            }
            else
            {
                PhotonNetwork.LoadLevel("VictoryScene");
            }
        }
        else if (GameMgr.redNexus == false)
        {
            if (GameMgr.isMaster)
            {
                PhotonNetwork.LoadLevel("VictoryScene");
            }
            else
            {
                PhotonNetwork.LoadLevel("DefeateScene");
            }
        }
        
       
        
        yield return null;
    }
}
