using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum EUnitState
    {
        Wait,
        Move
    }

    public EUnitState eUnitState;
}
