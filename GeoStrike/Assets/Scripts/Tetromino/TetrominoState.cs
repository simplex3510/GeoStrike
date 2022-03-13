using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrominoState : MonoBehaviour
{
    public enum ETetrominoState
    {
        FSM_InComplete, // 미완성 상태
        FSM_Normal,     // 유닛 소환 가능 상태
        FSM_Summoned    // 소환 완료 상태
    }

    private ETetrominoState eTetrominoState = ETetrominoState.FSM_InComplete;

    [SerializeField] private float buildTime;
    [SerializeField] private float currentTime = 0f;

    [HideInInspector] private UnitCreator unitCreation;

    private void Start()
    {
        if (unitCreation == null) { unitCreation = GetComponent<UnitCreator>(); }
        StartCoroutine(FSM());
    }

    public ETetrominoState GetState()
    {
        return eTetrominoState;
    }

    private void SetState(ETetrominoState _state)
    {
        eTetrominoState = _state;
    }

    // 건물 생성 FadeIn 효과
    IEnumerator CInCompleteColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color;
        while (eTetrominoState == ETetrominoState.FSM_InComplete)
        {
            currentTime += Time.deltaTime;

            color = spriteRenderer.color;
            color.a = (currentTime / buildTime) + 0.3f;
            spriteRenderer.color = color;
            yield return null;
        }
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(eTetrominoState.ToString());
        }
    }

    IEnumerator FSM_InComplete()
    {
        StartCoroutine(CInCompleteColor());
        yield return new WaitForSeconds(buildTime);
        Debug.Log("Build Complete");

        SetState(ETetrominoState.FSM_Normal);
    }

    IEnumerator FSM_Normal()
    {
        yield return new WaitForSeconds(0.5f);  // 건물완성 후 0.5초 뒤 유닛 생성

        if (GameMgr.instance.GetState() == EGameState.SpawnCount)
        {
            // 유닛 소환
            Debug.Log("Summone unit");
            unitCreation.UnitSpawn();
            SetState(ETetrominoState.FSM_Summoned);
        }
        yield return null;
    }

    // 배틀 타임이 될때까지 대기, 유닛이 이동되고 다시 소환대기 상태로 전환
    IEnumerator FSM_Summoned()
    {
        Debug.Log("Summoned");
        while (GameMgr.instance.GetState() == EGameState.SpawnCount)
        {   
            yield return null;
        }
        SetState(ETetrominoState.FSM_Normal);
    }
}
