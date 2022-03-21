using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviourPun
{
    LayerMask opponentLayerMask;

    EBuffandDebuff currentBuff = EBuffandDebuff.Damage;

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

    public void ChoiceBuffAttack()
    {
        currentBuff = EBuffandDebuff.Damage;
    }

    public void ChoiceBuffDefence()
    {
        currentBuff = EBuffandDebuff.Defence;
    }

    public void ChoiceBuffHaste()
    {
        currentBuff = EBuffandDebuff.Haste;
    }
}