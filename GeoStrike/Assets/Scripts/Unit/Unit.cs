using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum E_UnitState
    {
        Wait,
        Move
    }

    public E_UnitState E_unitState;
}
