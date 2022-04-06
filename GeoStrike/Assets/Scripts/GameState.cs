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

    public double startTime;
    public double currentTime;
    public double targetTime;

    public static readonly int STANDBYTIME = 5;

    private void Start()
    {
        StartCoroutine(EStandbyCount());
    }

    private void Update()
    {
        // ���� ��/�� ���
        if (GameMgr.instance.GetState() == EGameState.GameEnd)
        {
            GameEnd();
        }
    }

    //���� ���� �� �غ� �ð�(ī��Ʈ �ٿ�)
    public IEnumerator EStandbyCount()
    {
        startTime = PhotonNetwork.Time;
        currentTime = PhotonNetwork.Time;
        targetTime = startTime + STANDBYTIME;

        standbyCountObj.SetActive(true);

        while (currentTime <= targetTime)
        {
            
            countText.text = ((int)(targetTime - currentTime)).ToString();
            yield return null;
        }

        Debug.Log("Game Start");
        standbyCountObj.SetActive(false);
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
