using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameState : MonoBehaviour
{
    public static readonly int GAMESTATE_STANDBYTIME = 5;

    private void Start()
    {
        StartCoroutine(FSM());
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(GameMgr.instance.Get_State().ToString());
        }
    }

    // 게임 시작 전 5초 준비시간
    IEnumerator FSM_Standby()
    {
        yield return new WaitForSeconds(GAMESTATE_STANDBYTIME);
        Debug.Log("Game Start");
        GameMgr.instance.Set_State(EGameState.FSM_SpawnCount);
    }

    IEnumerator FSM_SpawnCount()
    {
        while (GameMgr.instance.Get_State() == EGameState.FSM_SpawnCount)
        {
            yield return null;
        }
    }

    IEnumerator FSM_Battle()
    {
        while(GameMgr.instance.Get_State() == EGameState.FSM_Battle)
        {
            yield return null;
        }
    }
}
