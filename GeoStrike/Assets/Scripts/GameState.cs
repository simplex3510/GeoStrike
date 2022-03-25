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

        // ���� ��/�� ���
        if (GameMgr.instance.GetState() == EGameState.GameEnd)
        {
            GameEnd();
        }
    }

    // ���� ���� �� �غ� �ð� (ī��Ʈ �ٿ�)
    IEnumerator EStandbyCount()
    {
        // SelectSpecialSkill Ȱ��ȭ�ϱ�
        while (0 <= stanbyCount /* && ���ÿϷ��ҋ����� */)
        {
            stanbyCount -= Time.deltaTime;
            yield return null;
        }
        // SelectSpecialSkill ��Ȱ��ȭ�ϱ�

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
        // 1.����� �ؼ����ı�
        // ����� �й�
        // ������ �¸�

        // 2.������ �ؼ��� �ı�
        // ����� �¸�
        // ������ �й�

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
