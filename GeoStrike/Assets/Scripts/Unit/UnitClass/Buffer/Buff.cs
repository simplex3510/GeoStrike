using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviourPun
{

    LayerMask opponentLayerMask;
    List<Collider2D> allyCollider2D = new List<Collider2D>();
    Unit ally;
    EBuffandDebuff currentBuff;
    float buffRange;

    // Buff Status Delta
    float buffDamage = 2f;

    protected void Awake()
    {
        opponentLayerMask = 1 << (int)EPlayer.Ally;
        buffRange = 5f;
    }

    private void OnTriggerEnter2D(Collider2D ally)
    {
        if(ally.gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            allyCollider2D.Add(ally);
            ally.GetComponent<Unit>().OnBuff(EBuffandDebuff.Damage, buffDamage);
        }

    }

    private void OnTriggerExit2D(Collider2D ally)
    {
        if (ally.gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            allyCollider2D.Remove(ally);
            ally.GetComponent<Unit>().OffBuff(EBuffandDebuff.Damage, buffDamage);
        }
    }
}