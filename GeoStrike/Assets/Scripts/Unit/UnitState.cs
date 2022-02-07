using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState : MonoBehaviour
{
    public enum EUnitState
    {
        FSM_Standby,
        FSM_SpawnBattleField,
        FSM_Move,
        FSM_Attack,
        FSM_Die
    }

    private Unit unit;
    [SerializeField] private EUnitState eUnitState = EUnitState.FSM_Standby;
    private static readonly int SPAWN_STANDBY = 1;

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

    // UnitField에 소환 되어 있을 때
    IEnumerator FSM_Standby()
    {
        while (GameMgr.instance.Get_State() == EGameState.FSM_SpawnCount)
        {
            // -> Unit 배치 구현하기
            yield return null;
        }
        Set_State(EUnitState.FSM_SpawnBattleField);
    }

    // BattleField로 소환된 뒤 x초 후 이동
    IEnumerator FSM_SpawnBattleField()
    {
        //  -> if 적이 감지되면 Attack모드 구현
        yield return new WaitForSeconds(SPAWN_STANDBY);

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
        // -> 공격
        yield return null;
    }

    IEnumerator FSM_Die()
    {
        // -> pool 반환
        yield return null;
    }
}
