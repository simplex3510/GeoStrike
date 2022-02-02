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

    public ETetrominoState eTetrominoState = ETetrominoState.FSM_InComplete;

    [SerializeField] private float buildTime;
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color color;

    [SerializeField] private UnitCreation unitCreation;
    [SerializeField] private Timer timer;

    private void Awake()
    {
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
        if (unitCreation == null) { unitCreation = GetComponent<UnitCreation>(); }
        if (timer == null) { timer = GameMgr.instance.canvas.GetComponentInChildren<Timer>(); }
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
        StartCoroutine(CInCompleteColor()); ;
        yield return new WaitForSeconds(buildTime);
        Debug.Log("Build Complete");

        Set_State(ETetrominoState.FSM_Normal);
    }

    IEnumerator FSM_Normal()
    {
        yield return new WaitForSeconds(1);

        // 건물 완성 후 1초 대기 시간을 가짐, 1초 대기 시간동안 배틀 시간이 되면 소환 불가 
        if (eTetrominoState == ETetrominoState.FSM_Normal) 
        {
            // 유닛 소환
            Debug.Log("Summone unit");
            unitCreation.UnitSpawn();
            Set_State(ETetrominoState.FSM_Summoned);
        }
    }

    IEnumerator FSM_Summoned()
    {
        Debug.Log("Summoned");
        while (timer.battleTimer != 0)
        {
            // battleTimer가 0이 될때까지 대기
            yield return null;
        }
        Set_State(ETetrominoState.FSM_Normal);
    }
}
