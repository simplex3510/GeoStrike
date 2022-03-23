using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class Timer : MonoBehaviour
{
    [Header("< World Time >")]
    [SerializeField] private Text worldTimeTXT;
    public double sec = 0f;
    private int min = 0;
    private readonly int MAX_SEC = 60;

    [Header("< Battle Timer >")]
    [SerializeField] public float battleTime;
    [SerializeField] private Slider slider;
    //public float battleTimer = 0f;
    //private int count = 1;

    //double sendtime;
    float lastBattleTime;
    float lastWorldTime;
    float currentTime;       // 현재 photon 시간
    //float playTime;        // currentTime - lastTime = 현재 게임 플레이한 시간


    private TranslocateField translocateField;
    private GameState gameState;
    private Detector detector;
    private KeySlotPanel keySlotPanel;

    private void Awake()
    {
        if (translocateField == null) { translocateField = FindObjectOfType<TranslocateField>(); }
        if (gameState == null)        { gameState = GetComponent<GameState>(); }
        if (detector == null)         { detector = GameObject.FindObjectOfType<Detector>(); }
        if (keySlotPanel == null)     { keySlotPanel = GameObject.FindObjectOfType<KeySlotPanel>(); }

        slider.maxValue = battleTime;

        currentTime = (float)PhotonNetwork.Time;
        lastBattleTime = (float)PhotonNetwork.Time;
        lastWorldTime = (float)PhotonNetwork.Time;
    }

    private void Update()
    {
        if (GameMgr.instance.GetState() != EGameState.Standby)
        {
            currentTime = (float)PhotonNetwork.Time;
            BattleTime();
            WorldTime();
        }
    }

    private void BattleTime()
    {
        slider.value = (float)currentTime - lastBattleTime;

        if (GameMgr.instance.GetState() != EGameState.SpawnCount && GameMgr.instance.GetState() != EGameState.GameEnd)
        {
            GameMgr.instance.SetState(EGameState.SpawnCount);
        }

        if (battleTime <= currentTime - lastBattleTime) 
        {
            //count++;
            lastBattleTime = (float)PhotonNetwork.Time;
            // (버퍼,디버퍼)유닛의 스킬 선택창 유지 방지
            #region 스킬 선책창 유지 방지
            if (detector.clickedUnit != null && detector.clickedObject.CompareTag("Unit") && detector.clickedObject.GetComponent<Unit>().unitState == EUnitState.Idle)
            {
                keySlotPanel.SetActiveFalseAll();
                detector.clickedUnit = null;
            }
            #endregion

            GameMgr.instance.SetState(EGameState.Battle);

            translocateField.TranslocateUnits();

            // 매 배틀 타임마다 게이지 최소,최대 값 설정
            // slider.minValue = battleTime * (count - 1);
            // slider.maxValue = battleTime * count;

            slider.value = 0f;
        } 
    }

    private void WorldTime()
    {
        if (1 <= currentTime - lastWorldTime)
        {
            lastWorldTime = (float)PhotonNetwork.Time;
            sec++;
        }

        if (sec >= MAX_SEC)
        {
            sec = 0;
            min++;
        }

        worldTimeTXT.text = $"{min:D2} : {(int)sec:D2}";
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting && PhotonNetwork.IsMasterClient)
    //    {
    //        sendtime = (float)PhotonNetwork.Time;
    //        stream.SendNext(sendtime);

    //        stream.SendNext(battleTimer);
    //        stream.SendNext(sec);
    //        stream.SendNext(min);
    //    }
    //    else
    //    {
    //        sendtime = (double)stream.ReceiveNext();
    //        float lag = Mathf.Abs((float)(PhotonNetwork.Time - sendtime));

    //        battleTimer = (float)stream.ReceiveNext();

    //        sec = (float)stream.ReceiveNext();
    //        sec += lag;

    //        min = (int)stream.ReceiveNext();
    //    }
    //}
}
