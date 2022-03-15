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
    [SerializeField] public float battleTime;
    [SerializeField] private Slider slider;
    public float battleTimer = 0f;

    double sendtime;

    private TranslocateField translocateField;
    private GameState gameState;
    private Detector detector;

    private void Awake()
    {
        if (translocateField == null) { translocateField = FindObjectOfType<TranslocateField>(); }
        if (gameState == null) { gameState = GetComponent<GameState>(); }
        if (detector == null) { detector = GameObject.FindObjectOfType<Detector>(); }

        slider.maxValue = battleTime;
    }

    private void Update()
    {
        if (GameMgr.instance.GetState() != EGameState.Standby)
        {
            WorldTime();
            BattleTime();
        }
    }

    private void BattleTime()
    {
        battleTimer += Time.deltaTime;
        slider.value = battleTimer;

        if (GameMgr.instance.GetState() != EGameState.SpawnCount)
        {
            GameMgr.instance.SetState(EGameState.SpawnCount);
        }

        if (battleTimer >= battleTime) 
        {
            // (버퍼,디버퍼)유닛의 스킬 선택창 유지 방지
            if (detector.clickedObject.CompareTag("Unit") && detector.clickedObject.GetComponent<Unit>().unitState == EUnitState.Idle)
            {
                detector.clickedObject = null;
            }

            GameMgr.instance.SetState(EGameState.Battle);
            battleTimer = 0f;

            translocateField.TranslocateUnits();
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
