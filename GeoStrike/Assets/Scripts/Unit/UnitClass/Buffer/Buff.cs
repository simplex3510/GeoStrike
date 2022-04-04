using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviourPun
{

    public EBuffandDebuff CurrentBuff 
    {
        get { return currentBuff; }
        set
        {
            switch (value)
            {
                case EBuffandDebuff.Damage:
                    break;
                case EBuffandDebuff.Defence:
                    break;
                case EBuffandDebuff.Haste:
                    break;
                default:
                    break;
            }
        }    
    } 

    List<GameObject> buffedAlly = new List<GameObject>();
    EBuffandDebuff currentBuff = EBuffandDebuff.Damage;

    // Buff Status Delta
    float buffDeltaStatus = 2f;

    private void OnDisable()
    {
        for (int i = 0; i < buffedAlly.Count; i++)
        {
            if(buffedAlly[i] == this.gameObject)
            {
                continue;
            }

            buffedAlly[i].GetComponent<Unit>().OffBuff((int)currentBuff, buffDeltaStatus);
        }

        buffedAlly.Clear();
    }

    private void OnTriggerEnter(Collider ally)
    {
        if(photonView.IsMine && ally.GetComponent<Unit>() != null)
        {
            buffedAlly.Add(ally.gameObject);
            ally.GetComponent<Unit>().OnBuff((int)currentBuff, buffDeltaStatus);
        }

    }

    private void OnTriggerExit(Collider ally)
    {
        if (photonView.IsMine && ally.GetComponent<Unit>() != null)
        {
            buffedAlly.Remove(ally.gameObject);
            ally.GetComponent<Unit>().OffBuff((int)currentBuff, buffDeltaStatus);
        }
    }
}