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
        // ���� ��/�� ���
        if (GameMgr.instance.GetState() == EGameState.GameEnd)
        {
            GameEnd();
        }
    }

    //���� ���� �� �غ� �ð�(ī��Ʈ �ٿ�)
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
            countText.text = ((int)(PhotonNetwork.Time - startTime)).ToString();
            slider.value = (int)(PhotonNetwork.Time - startTime);
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
