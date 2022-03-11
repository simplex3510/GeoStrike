using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum EGameState
{
    FSM_Standby,        // ���� ���� �� �غ� �ܰ�
    FSM_SpawnCount,     // ���� �ʵ��� ���� ��ġ ������ �ð� 0s ~ Xs;
    FSM_Battle,         // ���� ��Ʋ�ʵ�� �̵�
    FSM_GameEnd,        // ���� ����
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

    [SerializeField] private EGameState eGameState = EGameState.FSM_Standby;

    public static bool isMaster = false;

    // true = ����, false = �ı�
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

        if (PhotonNetwork.IsMasterClient)
        {
            isMaster = true;
        }
        else
        {
            isMaster = false;
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
