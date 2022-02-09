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
        StartCoroutine(CInCompleteColor());
        yield return new WaitForSeconds(buildTime);
        Debug.Log("Build Complete");

        Set_State(ETetrominoState.FSM_Normal);
    }

    IEnumerator FSM_Normal()
    {
        // 1�� ���ð� �� ���� ��ȯ
        yield return new WaitForSeconds(1);
        GameMgr.instance.SetState(EGameState.FSM_SpawnCount);

        if (GameMgr.instance.GetState() == EGameState.FSM_SpawnCount)
        {
            // ���� ��ȯ
            Debug.Log("Summone unit");
            unitCreation.UnitSpawn();
            Set_State(ETetrominoState.FSM_Summoned);
        }
    }

    // ��Ʋ Ÿ���� �ɶ����� ���, ������ �̵��ǰ� �ٽ� ��ȯ��� ���·� ��ȯ
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
