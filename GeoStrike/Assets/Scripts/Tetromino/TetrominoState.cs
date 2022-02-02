using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrominoState : MonoBehaviour
{
    public enum ETetrominoState
    {
        FSM_InComplete, // �̿ϼ� ����
        FSM_Normal,     // ���� ��ȯ ���� ����
        FSM_Summoned    // ��ȯ �Ϸ� ����
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

    // �ǹ� ���� FadeIn ȿ��
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

        // �ǹ� �ϼ� �� 1�� ��� �ð��� ����, 1�� ��� �ð����� ��Ʋ �ð��� �Ǹ� ��ȯ �Ұ� 
        if (eTetrominoState == ETetrominoState.FSM_Normal) 
        {
            // ���� ��ȯ
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
            // battleTimer�� 0�� �ɶ����� ���
            yield return null;
        }
        Set_State(ETetrominoState.FSM_Normal);
    }
}
