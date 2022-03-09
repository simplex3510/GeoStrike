using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class Timer : MonoBehaviour, IPunObservable
{
    [Header("< World Time >")]
    [SerializeField] private Text worldTimeTXT;
    public float sec = 0f;
    private int min = 0;
    private readonly int MAX_SEC = 59;

    [Header("< Battle Timer >")]
    [SerializeField] private Text battleTimeTXT;
    [SerializeField] public float battleTime;
    public float battleTimer = 0f;

    double sendtime;

    private TranslocateField translocateField;
    private GameState gameState;

    private void Awake()
    {
        if (translocateField == null) { translocateField = FindObjectOfType<TranslocateField>(); }
        if (gameState == null) { gameState = GetComponent<GameState>(); }
    }

    private void Update()
    {
        if (GameMgr.instance.GetState() != EGameState.FSM_Standby)
        {
            WorldTime();
            BattleTime();
        }
    }

    private void BattleTime()
    {
        battleTimer += Time.deltaTime;
        battleTimeTXT.text = $"{(int)battleTimer:D}s";

        if (battleTimer >= battleTime) 
        {
            GameMgr.instance.SetState(EGameState.FSM_Battle);
            battleTimer = 0f;

            translocateField.TranslocateUnits();
            translocateField.gameObject.GetComponent<UnitTileContainer>().TileAllClear();
        }
    }

    private void WorldTime()
    {
        sec += Time.deltaTime;
        worldTimeTXT.text = $"{min:D2} : {(int)sec:D2}";

        if(sec >= MAX_SEC)
        {
            sec = 0;
            min++;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            sendtime = (float)PhotonNetwork.Time;
            stream.SendNext(sendtime);

            stream.SendNext(battleTimer);
            stream.SendNext(sec);
            stream.SendNext(min);
        }
        else
        {
            sendtime = (double)stream.ReceiveNext();
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - sendtime));

            battleTimer = (float)stream.ReceiveNext();

            sec = (float)stream.ReceiveNext();
            sec += lag;

            min = (int)stream.ReceiveNext();

        }
    }
}
