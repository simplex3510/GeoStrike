using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslocateField : MonoBehaviour
{
    // Instantiate (temp)
    [SerializeField] private GameObject unitPrefabs;

    // Unit state
    [SerializeField] private List<Unit> unitList = new List<Unit>();

    // Waiting unit translocate
    [SerializeField] private Timer playerTime;
    [SerializeField] private GameObject moveToBattleField;

    // Waiting unit parent
    [SerializeField] private GameObject parent;
    private Transform parentOriginPos;

    private void Awake()
    {
        parentOriginPos = parent.transform;
    }

    private void Start()
    {
        // for test
        Instantiate(unitPrefabs, new Vector3(-4f, 0f, -1f), Quaternion.identity);
        Instantiate(unitPrefabs, new Vector3(-4f, 1f, -1f), Quaternion.identity);
        Instantiate(unitPrefabs, new Vector3(-4f, -1f, -1f), Quaternion.identity);
    }

    private void Update()
    {
        TranslocateUnits();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Unit"))
        {
            unitList.Add(_collision.gameObject.GetComponent<Unit>());
            _collision.gameObject.transform.SetParent(parent.transform);
        }
    }

    private void TranslocateUnits() 
    {
        if (playerTime.isReady)
        {
            playerTime.isReady = false;
            parent.transform.position = moveToBattleField.transform.position;
            ClearList();
        }
    }

    private void ClearList()
    {
        for (int idx = 0; idx < unitList.Count; idx++)
        {
            //unitList[idx].gameObject.transform.parent = null;
        }
        unitList.Clear();
        parent.transform.position = parentOriginPos.position;
    }
}
