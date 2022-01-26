using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    #region singleton
    // public static IconMgr instance { get; private set; }
    private static GameMgr _instance = null;
    public static GameMgr instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(GameMgr))
                    as GameMgr;
                if (!_instance)
                {
                    Debug.LogError(" _instance null");
                    return null;
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError(" UIMgr duplicated.  ");
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    // Slot °ü¸®
    public List<Tetromino> m_tetrtominoList = new List<Tetromino>();
    public List<TetrominoSlot> m_slotList = new List<TetrominoSlot>();
}
