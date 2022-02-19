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
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color color;

    [HideInInspector] private UnitCreator unitCreation;

    private void Awake()
    {
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
        if (unitCreation == null) { unitCreation = GetComponent<UnitCreator>(); }
    }

    private void Start()
    {
        StartCoroutine(FSM());
    }

    private void Set_State(ETetrominoState _state)
    {
        eTetrominoState = _state;
    }

    // 건물 생성 FadeIn 효과
    IEnumerator CInCompleteColor()
    {
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

        Set_State(ETetrominoState.FSM_Normal);
    }

    IEnumerator FSM_Normal()
    {
        // 1초 대기시간 후 유닛 소환
        yield return new WaitForSeconds(1);
        GameMgr.instance.SetState(EGameState.FSM_SpawnCount);

        if (GameMgr.instance.GetState() == EGameState.FSM_SpawnCount)
        {
            // 유닛 소환
            Debug.Log("Summone unit");
            unitCreation.UnitSpawn();
            Set_State(ETetrominoState.FSM_Summoned);
        }
    }

    // 배틀 타임이 될때까지 대기, 유닛이 이동되고 다시 소환대기 상태로 전환
    IEnumerator FSM_Summoned()
    {
        Debug.Log("Summoned");
        while (GameMgr.instance.GetState() == EGameState.FSM_SpawnCount)
        {   
            yield return null;
        }
        Set_State(ETetrominoState.FSM_Normal);
    }
}
