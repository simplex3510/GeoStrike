//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UnitState : MonoBehaviour
//{
//    public enum EUnitState
//    {
//        FSM_Standby,
//        FSM_SpawnBattleField,
//        FSM_Move,
//        FSM_Attack,
//        FSM_Die
//    }

//    private Unit unit;
//    [SerializeField] private EUnitState eUnitState = EUnitState.FSM_Standby;
//    private static readonly int SPAWN_STANDBY = 1;

//    private void Awake()
//    {
//        if (unit == null) { unit = GetComponent<Unit>(); }
//    }

//    private void Start()
//    {
//        StartCoroutine(FSM());
//    }

//    public EUnitState GetState()
//    {
//        return eUnitState;
//    }

//    private void SetState(EUnitState _state)
//    {
//        eUnitState = _state;
//    }

//    IEnumerator FSM()
//    {
//        while (true)
//        {
//            yield return StartCoroutine(eUnitState.ToString());
//        }
//    }

//    // UnitField�� ��ȯ �Ǿ� ���� ��
//    IEnumerator FSM_Standby()
//    {
//        unit.unitMovement.Set_FreezePosition();
//        while (GameMgr.instance.GetState() == EGameState.FSM_SpawnCount)
//        {
//            // -> Unit ��ġ �����ϱ�
//            yield return null;
//        }
//        SetState(EUnitState.FSM_SpawnBattleField);
//    }

//    // BattleField�� ��ȯ�� �� x�� �� �̵�
//    IEnumerator FSM_SpawnBattleField()
//    {
//        unit.unitMovement.Set_RevertPosition();
//        //  -> if ���� �����Ǹ� Attack��� ����
//        yield return new WaitForSeconds(SPAWN_STANDBY);

//        SetState(EUnitState.FSM_Move);
//    }

//    IEnumerator FSM_Move()
//    {
//        while(eUnitState == EUnitState.FSM_Move)
//        {
//            unit.unitMovement.Move();
//            yield return null;
//        }
//    }
    
//    IEnumerator FSM_Attack()
//    {
//        // -> ����
//        yield return null;
//    }

//    IEnumerator FSM_Die()
//    {
//        // -> pool ��ȯ
//        yield return null;
//    }
//}
