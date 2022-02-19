using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum EGameState
{
    FSM_Standby,        // 게임 시작 전 준비 단계
    FSM_SpawnCount,     // 실시간 유닛 스폰 시간 0s ~ 스폰시간s
    FSM_Battle,         // 유닛 스폰시간
    FSM_GameEnd,        // 게임 종료
}

public class GameMgr : MonoBehaviourPun
{
    // public static IconMgr instance { get; private set; }
    private static GameMgr _instance = null;
    public static GameMgr instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(GameMgr))
                    as GameMgr;
                if (!_instance)
                {
                    Debug.LogError(" _instance null");
                    return null;
                }
            }
            return _instance;
        }
    }

    public static bool isMaster = false;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError(" Mgr duplicated.  ");
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            isMaster = true;
        }
        else
        {
            isMaster = false;
        }
    }

    [SerializeField] private EGameState eGameState = EGameState.FSM_Standby;

    public Grid grid;
    public Canvas canvas;


    public EGameState GetState()
    {
        return eGameState;
    }

    public void SetState(EGameState _state)
    {
        eGameState = _state;
    }
}
