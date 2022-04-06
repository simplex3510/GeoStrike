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

    // PhotonTime
    float lastBattleTime;
    float lastWorldTime;
    float currentTime;       // 현재 photon 시간

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
    }

    private void Start()
    {
        StartCoroutine(CGameTimer());
    }

    IEnumerator CGameTimer()
    {
        yield return StartCoroutine(gameState.EStandbyCount());

        currentTime = (float)PhotonNetwork.Time;
        lastBattleTime = (float)PhotonNetwork.Time;
        lastWorldTime = (float)PhotonNetwork.Time;

        while (GameMgr.instance.GetState() != EGameState.Standby)
        {
            currentTime = (float)PhotonNetwork.Time;
            BattleTime();
            WorldTime();
            yield return null;
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
}
