using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState : MonoBehaviour
{
    public enum EUnitState
    {
        FSM_Normal,
        FSM_Move,
        FSM_Attack,
        FSM_Die
    }

    private Unit unit;
    [SerializeField] private EUnitState eUnitState = EUnitState.FSM_Normal;

    private void Awake()
    {
        if (unit == null) { unit = GetComponent<Unit>(); }
    }

    private void Start()
    {
        StartCoroutine(FSM());
    }

    private void Set_State(EUnitState _state)
    {
        eUnitState = _state;
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(eUnitState.ToString());
        }
    }

    // ��ȯ�ǰ� 1�� ����� �̵�
    IEnumerator FSM_Normal()
    {
        yield return new WaitForSeconds(1);
        Set_State(EUnitState.FSM_Move);
    }

    IEnumerator FSM_Move()
    {
        while(eUnitState == EUnitState.FSM_Move)
        {
            unit.unitMovement.Move();
            yield return null;
        }
    }

    IEnumerator FSM_Attack()
    {
        // ����
        yield return null;
    }

    IEnumerator FSM_Die()
    {
        // pool ��ȯ
        yield return null;
    }
}
