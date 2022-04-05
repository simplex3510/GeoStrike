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
    //public GameObject standbyCountObj;
    //public Text countText;
    //public float count;

    //public static readonly int GAMESTATE_STANDBYTIME = 5;

    //private void Start()
    //{
    //    StartCoroutine(EStandbyCount());
    //}

    private void Update()
    {
        // °ÔÀÓ ½Â/ÆÐ °á°ú
        if (GameMgr.instance.GetState() == EGameState.GameEnd)
        {
            GameEnd();
        }
    }

    // °ÔÀÓ ½ÃÀÛ Àü ÁØºñ ½Ã°£ (Ä«¿îÆ® ´Ù¿î)
    //public IEnumerator EStandbyCount()
    //{
    //    count = GAMESTATE_STANDBYTIME;
    //    standbyCountObj.SetActive(true);

    //    while (count >= 0)
    //    {
    //        count -= Time.deltaTime;
    //        countText.text = ((int)count).ToString();
    //        yield return null;
    //    }

    //    Debug.Log("Game Start");
    //    standbyCountObj.SetActive(false);
    //    GameMgr.instance.SetState(EGameState.SpawnCount);
    //}

    public void GameEnd()
    {
        // 1.ºí·çÆÀ ³Ø¼­½ºÆÄ±«
        // ºí·çÆÀ ÆÐ¹è
        // ·¹µåÆÀ ½Â¸®

        // 2.·¹µåÆÀ ³Ø¼­½º ÆÄ±«
        // ºí·çÆÀ ½Â¸®
        // ·¹µåÆÀ ÆÐ¹è

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
