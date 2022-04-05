using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField] Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        PhotonNetwork.LoadLevel("LoadScene");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }

    IEnumerator LoadSceneProgress()
    {
        PhotonNetwork.LoadLevel(nextScene);
        //AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        //op.allowSceneActivation = false;

        //float timer = 0f;

        while(true/*!op.allowSceneActivation*/)
        {
            progressBar.fillAmount = PhotonNetwork.LevelLoadingProgress;
            //if(true/*PhotonNetwork.LevelLoadingProgress < 0.9f*/)
            //{
            //}
            //else
            //{
            //    timer += Time.deltaTime;
            //    progressBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);
            //    if(1.0f <= progressBar.fillAmount)
            //    {
            //        //op.allowSceneActivation = true;
            //        yield break;
            //    }
            //}

            if (1.0f <= progressBar.fillAmount)
            {
                //op.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }
    }
}
