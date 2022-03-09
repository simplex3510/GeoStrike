using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum EGameState
{
    FSM_Standby,        // 게임 시작 전 준비 단계
    FSM_SpawnCount,     // 유닛 필드의 유닛 배치 가능한 시간 0s ~ Xs;
    FSM_Battle,         // 유닛 배틀필드로 이동
    FSM_GameEnd,        // 게임 종료
}

[DefaultExecutionOrder(201)]
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
