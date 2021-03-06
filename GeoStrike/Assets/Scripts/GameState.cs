using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


[DefaultExecutionOrder(202)]
public class GameState : MonoBehaviourPun
{
    public GameObject standbyCountObj;
    public Text countText;

    public double startTime = 0f;
    public double targetTime = 0f;

    public static readonly int STANDBYTIME = 10;

    private void Start()
    {
        StartCoroutine(EStandbyCount());
    }

    private void Update()
    {
        // 게임 승/패 결과
        if (GameMgr.instance.GetState() == EGameState.GameEnd)
        {
            GameEnd();
        }
    }

    //게임 시작 전 준비 시간(카운트 다운)
    public IEnumerator EStandbyCount()
    {   
        if(PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time;
            targetTime = startTime + STANDBYTIME;
            photonView.RPC("SyncStartTime", RpcTarget.Others, startTime, targetTime);
        }
        else
        {
            while (startTime == 0f)
            {
                yield return null;
            }
        }

        Slider slider = standbyCountObj.GetComponentInChildren<Slider>();
        slider.maxValue = STANDBYTIME;

        standbyCountObj.SetActive(true);

        while (PhotonNetwork.Time - startTime <= STANDBYTIME)
        {
            if(1f <= targetTime - PhotonNetwork.Time)
            {
                countText.text = "Loading...";
            }
            else
            {
                countText.text = "Game Start";
            }
            slider.value = (float)(PhotonNetwork.Time - startTime);
            yield return null;
        }

        Debug.Log("Game Start");
        standbyCountObj.SetActive(false);
        GameMgr.instance.SetState(EGameState.SpawnCount);
    }

    [PunRPC]
    void SyncStartTime(double _startTime, double _targetTime)
    {
        startTime = _startTime;
        targetTime = _targetTime;
    }

    public void GameEnd()
    {
        // 1.블루팀 넥서스파괴
        // 블루팀 패배
        // 레드팀 승리

        // 2.레드팀 넥서스 파괴
        // 블루팀 승리
        // 레드팀 패배

        if (GameMgr.blueNexus == false)
        {
            if (GameMgr.isMaster)
            {
                PhotonNetwork.LoadLevel("DefeateScene");
            }
            else
            {
                PhotonNetwork.LoadLevel("VictoryScene");
            }
        }
        else if (GameMgr.redNexus == false)
        {
            if (GameMgr.isMaster)
            {
                PhotonNetwork.LoadLevel("VictoryScene");
            }
            else
            {
                PhotonNetwork.LoadLevel("DefeateScene");
            }
        }
    }
}
