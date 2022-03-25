using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


[DefaultExecutionOrder(202)]
public class GameState : MonoBehaviourPun
{
    public static readonly int GAMESTATE_STANDBYTIME = 5;
    public static readonly int GAMESTATE_COUNT = 1;
    public float stanbyCount; 

    private void Start()
    {
        StartCoroutine(EStandbyCount());
    }

    private void Update()
    {
        //if (GameMgr.instance.GetState() == EGameState.Standby)
        //{
        //    stanbyCount = stanbyCount - Time.deltaTime;
        //}

        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameMgr.redNexus = false;
            GameMgr.instance.SetState(EGameState.GameEnd);
        }

        // 게임 승/패 결과
        if (GameMgr.instance.GetState() == EGameState.GameEnd)
        {
            GameEnd();
        }
    }

    // 게임 시작 전 준비 시간 (카운트 다운)
    IEnumerator EStandbyCount()
    {
        // SelectSpecialSkill 활성화하기
        while (0 <= stanbyCount /* && 선택완료할떄까지 */)
        {
            stanbyCount -= Time.deltaTime;
            yield return null;
        }
        // SelectSpecialSkill 비활성화하기

        //-------------------------old-----------------------------
        //for(int count = GAMESTATE_STANDBYTIME; count > 0; count--)
        //{
        //    //standbyCount.stanbyCountTXT.text = string.Format("{0}", count);
        //    yield return new WaitForSeconds(GAMESTATE_COUNT);
        //}

        ////standbyCount.gameObject.SetActive(false);

        Debug.Log("Game Start");
        GameMgr.instance.SetState(EGameState.SpawnCount);
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
