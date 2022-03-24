using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum EGameState
{
    Standby,        // 게임 시작 전 준비 단계
    SpawnCount,     // 유닛 필드의 유닛 배치 가능한 시간 0s ~ Xs;
    Battle,         // 유닛 배틀필드로 이동
    GameEnd,        // 게임 종료
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

    [SerializeField] private EGameState eGameState = EGameState.SpawnCount;

    public static bool isMaster = false;

    // true = 존재, false = 파괴
    public static bool blueNexus = true;
    public static bool redNexus = true;

    public Grid grid;
    public Canvas canvas;

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

        isMaster = PhotonNetwork.IsMasterClient;

        if(isMaster)
        {
            PhotonNetwork.Instantiate("Tower/Nexus_Blue",  new Vector3(-33.12f, 0,     0), Quaternion.Euler(90f, 0, 0));
            PhotonNetwork.Instantiate("Tower/Turret_Blue", new Vector3(-15.62f, 0, 3.35f), Quaternion.Euler(90f, 0, 0));
        }
        else
        {
            PhotonNetwork.Instantiate("Tower/Nexus_Red",  new Vector3(33.12f, 0,      0), Quaternion.Euler(90f, 0, 0));
            PhotonNetwork.Instantiate("Tower/Turret_Red", new Vector3(15.62f, 0, -3.35f), Quaternion.Euler(90f, 0, 0));
        }
    }



    public EGameState GetState()
    {
        return eGameState;
    }

    [PunRPC]
    public void SetState(EGameState _state)
    {
        eGameState = _state;
    }
}
