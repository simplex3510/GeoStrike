using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EGameState
{
    FSM_Standby,        // ���� ���� �� �غ� �ܰ�
    FSM_SpawnCount,     // �ǽð� ���� ���� �ð� 0s ~ �����ð�s
    FSM_Battle,         // ���� �����ð�
    FSM_GameEnd,        // ���� ����
}

public class GameMgr : MonoBehaviour
{
    #region singleton
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
    }
    #endregion

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
