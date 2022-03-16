using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviourPun
{
    LayerMask opponentLayerMask;

    // Buff Status Delta
    float buffDamage = 2f;

    protected void Awake()
    {
        opponentLayerMask = 1 << (int)EPlayer.Ally;
    }

    private void OnTriggerEnter2D(Collider2D ally)
    {
        if(ally.gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            ally.GetComponent<Unit>().OnBuff(EBuffandDebuff.Damage, buffDamage);
        }

    }

    private void OnTriggerExit2D(Collider2D ally)
    {
        if (ally.gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            ally.GetComponent<Unit>().OffBuff(EBuffandDebuff.Damage, buffDamage);
        }
    }
}