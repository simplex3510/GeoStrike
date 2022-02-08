using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameState : MonoBehaviour
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
            yield return StartCoroutine(GameMgr.instance.Get_State().ToString());
        }
    }

    // ���� ���� �� �غ� �ð� (ī��Ʈ �ٿ�)
    IEnumerator FSM_Standby()
    {
        for(int count = GAMESTATE_STANDBYTIME; count > 0; count--)
        {
            standbyCount.stanbyCountTXT.text = string.Format("{0}", count);
            yield return new WaitForSeconds(GAMESTATE_COUNT);
        }
        standbyCount.gameObject.SetActive(false);

        Debug.Log("Game Start");
        GameMgr.instance.Set_State(EGameState.FSM_SpawnCount);
    }

    // BattleTimer 0s ~ Xs ����
    IEnumerator FSM_SpawnCount()
    {
        while (GameMgr.instance.Get_State() == EGameState.FSM_SpawnCount)
        {
            yield return null;
        }
    }

    // 0s�� �Ǵ� ���� (SpawnTime)
    IEnumerator FSM_Battle()
    {
        while(GameMgr.instance.Get_State() == EGameState.FSM_Battle)
        {
            yield return null;
        }
    }
}
