using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviourPun
{
    LayerMask opponentLayerMask;

    EBuffandDebuff currentBuff = EBuffandDebuff.Damage;
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

    // Buff Status Delta
    float buffDeltaStatus = 2f;

    protected void Awake()
    {
        opponentLayerMask = 1 << (int)EPlayer.Ally;
    }

    private void OnTriggerEnter(Collider ally)
    {
        if(ally.gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            ally.GetComponent<Unit>().OnBuff((int)currentBuff, buffDeltaStatus);
        }

    }

    private void OnTriggerExit(Collider ally)
    {
        if (ally.gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            ally.GetComponent<Unit>().OffBuff((int)currentBuff, buffDeltaStatus);
        }
    }
}