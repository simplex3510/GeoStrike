using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : MonoBehaviour
{
    public enum EBuildingState
    {
        FSM_InComplete, // �̿ϼ� ����
        FSM_Normal,     // ���� ��ȯ ���� ����
        FSM_Summoned    // ��ȯ �Ϸ� ����
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
        // �̿ϼ��� ��ȯ�� ���¿����� ���� or ���İ� ��ȭ�ֱ�
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
        // ���� ��ȯ
        Debug.Log("Summone unit");
        Set_State(EBuildingState.FSM_Summoned);
    }

    IEnumerator FSM_Summoned()
    {
        // ���
        Debug.Log("Summoned");
        yield return new WaitForSeconds(20);
        Set_State(EBuildingState.FSM_Normal);
    }
}
