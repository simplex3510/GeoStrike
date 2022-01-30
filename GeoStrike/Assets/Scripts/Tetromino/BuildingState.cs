using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : MonoBehaviour
{
    public enum EBuildingState
    {
        FSM_InComplete, // 미완성 상태
        FSM_Normal,     // 유닛 소환 가능 상태
        FSM_Summoned    // 소환 완료 상태
    }

    public EBuildingState eBuildingState = EBuildingState.FSM_InComplete;

    [SerializeField] private float BuildTime;

    private void Start()
    {
        StartCoroutine(FSM());
    }

    private void Set_State(EBuildingState _state)
    {
        eBuildingState = _state;
    }

    private void Set_InCompleteColor()
    {
        // 미완성과 소환된 상태에따라 색상 or 알파값 변화주기
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(eBuildingState.ToString());
        }
    }

    IEnumerator FSM_InComplete()
    {
        yield return new WaitForSeconds(BuildTime);
        Debug.Log("Build Complete");

        Set_State(EBuildingState.FSM_Normal);
    }

    IEnumerator FSM_Normal()
    {
        yield return null;
        // 유닛 소환
        Debug.Log("Summone unit");
        Set_State(EBuildingState.FSM_Summoned);
    }

    IEnumerator FSM_Summoned()
    {
        // 대기
        Debug.Log("Summoned");
        yield return new WaitForSeconds(20);
        Set_State(EBuildingState.FSM_Normal);
    }
}
