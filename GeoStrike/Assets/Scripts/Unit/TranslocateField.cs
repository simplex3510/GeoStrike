using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslocateField : MonoBehaviour
{
    // Instantiate (temp)
    [SerializeField] private GameObject m_UnitPrefabs;

    // Unit state
    [SerializeField] private List<Unit> m_unitList = new List<Unit>();

    // Waiting unit translocate
    [SerializeField] private Timer m_playerTime;
    [SerializeField] private GameObject m_moveToBattleField;

    // Waiting unit parent
    [SerializeField] private GameObject m_parent;
    private Transform m_parentOriginPos;

    private void Awake()
    {
        m_parentOriginPos = m_parent.transform;
    }

    private void Start()
    {
        // for test
        Instantiate(m_UnitPrefabs, new Vector3(-4f, 0f, -1f), Quaternion.identity);
        Instantiate(m_UnitPrefabs, new Vector3(-4f, 1f, -1f), Quaternion.identity);
        Instantiate(m_UnitPrefabs, new Vector3(-4f, -1f, -1f), Quaternion.identity);
    }

    private void Update()
    {
        TranslocateUnits();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Unit"))
        {
            m_unitList.Add(_collision.gameObject.GetComponent<Unit>());
            _collision.gameObject.transform.SetParent(m_parent.transform);
        }
    }

    private void TranslocateUnits() 
    {
        if (m_playerTime.m_isReady)
        {
            m_playerTime.m_isReady = false;
            m_parent.transform.position = m_moveToBattleField.transform.position;
            ClearList();
        }
    }

    private void ClearList()
    {
        for (int idx = 0; idx < m_unitList.Count; idx++)
        {
            m_unitList[idx].gameObject.transform.parent = null;
        }
        m_unitList.Clear();
        m_parent.transform.position = m_parentOriginPos.position;
    }
}
