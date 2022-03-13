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

    // �ǹ� ���� FadeIn ȿ��
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
        yield return new WaitForSeconds(0.5f);  // �ǹ��ϼ� �� 0.5�� �� ���� ����

        if (GameMgr.instance.GetState() == EGameState.SpawnCount)
        {
            // ���� ��ȯ
            Debug.Log("Summone unit");
            unitCreation.UnitSpawn();
            SetState(ETetrominoState.FSM_Summoned);
        }
        yield return null;
    }

    // ��Ʋ Ÿ���� �ɶ����� ���, ������ �̵��ǰ� �ٽ� ��ȯ��� ���·� ��ȯ
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
