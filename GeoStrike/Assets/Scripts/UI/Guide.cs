using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guide : MonoBehaviour
{
    public void GoToGuide()
    {
        SceneManager.LoadScene("GuideScene");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("ConnectScene");
    }
}
