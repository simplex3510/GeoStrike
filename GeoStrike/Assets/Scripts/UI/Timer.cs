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
    double lastTime;    // photon�� �ð� ���� ��
    double photonTime;  // ���� photon �ð�
    double playTime;    // lastTime - photonTime = ���� ���� �÷����� �ð�


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
            // (����,�����)������ ��ų ����â ���� ����
            #region ��ų ��åâ ���� ����
            if (detector.clickedUnit != null && detector.clickedObject.CompareTag("Unit") && detector.clickedObject.GetComponent<Unit>().unitState == EUnitState.Idle)
            {
                keySlotPanel.SetActiveFalseAll();
                detector.clickedUnit = null;
            }
            #endregion

            GameMgr.instance.SetState(EGameState.Battle);

            translocateField.TranslocateUnits();

            // �� ��Ʋ Ÿ�Ӹ��� ������ �ּ�,�ִ� �� ����
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
