using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class TimeMgr : MonoBehaviour, IPunObservable
{
    public Text timerText;

    double sendtime;
    float sec = 55f;
    int min;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient && stream.IsWriting)
        {
            print("this");
            sendtime = (float)PhotonNetwork.Time;
            stream.SendNext(sendtime);
            stream.SendNext(sec);
            stream.SendNext(min);
        }
        else
        {
            print("this");
            sendtime = (double)stream.ReceiveNext();
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - sendtime));

            sec = (float)stream.ReceiveNext();
            sec -= lag;

            //print(lag);

            min = (int)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        sec += Time.deltaTime;
        if (60.0 <= sec)
        {
            min++;
            sec = 0.0f;
        }
        timerText.text = $"{(int)(min):d2}:{(int)sec:d2}";
    }
}
