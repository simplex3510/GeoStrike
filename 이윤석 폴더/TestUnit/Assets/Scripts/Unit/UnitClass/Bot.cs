using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bot : Unit
{

    protected override void Awake()
    {
        base.Awake();
        print("Bot Awake");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        unitState = EUnitState.Idle;
    }

    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //}

    protected override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
    
}