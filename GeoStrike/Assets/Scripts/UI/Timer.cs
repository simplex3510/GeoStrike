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
    private int count = 1;

    //double sendtime;
    double lastTime;    // photon의 시간 고정 값
    double photonTime;  // 현재 photon 시간
    double playTime;    // lastTime - photonTime = 현재 게임 플레이한 시간


    private TranslocateField translocateField;
    private GameState gameState;
    private Detector detector;
    private KeySlotPanel keySlotPanel;

    private void Awake()
    {
        if (translocateField == null) { translocateField = FindObjectOfType<TranslocateField>(); }
        if (gameState == null) { gameState = GetComponent<GameState>(); }
        if (detector == null) { detector = GameObject.FindObjectOfType<Detector>(); }
        if (keySlotPanel == null) { keySlotPanel = GameObject.FindObjectOfType<KeySlotPanel>(); }

        slider.maxValue = battleTime;
    }

    private void Start()
    {
        lastTime = PhotonNetwork.Time;
    }

    private void Update()
    {
        if (GameMgr.instance.GetState() != EGameState.Standby)
        {
            photonTime = PhotonNetwork.Time;
            WorldTime();
            BattleTime();
        }
    }

    private void BattleTime()
    {
        slider.value = (float)playTime;

        if (GameMgr.instance.GetState() != EGameState.SpawnCount && GameMgr.instance.GetState() != EGameState.GameEnd)
        {
            GameMgr.instance.SetState(EGameState.SpawnCount);
        }

        if (playTime >= battleTime * count) 
        {
            count++;
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
            slider.minValue = battleTime * (count - 1);
            slider.maxValue = battleTime * count;
        } 
    }

    private void WorldTime()
    {
        playTime = photonTime - lastTime;
        sec = playTime - (min * 60);
      
        worldTimeTXT.text = $"{min:D2} : {(int)sec:D2}";

        if(sec >= MAX_SEC)
        {
            min++;
        }

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
