using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public int x, y;    // 노드의 좌표값  
    public int G;       // G : 시작으로부터 이동했던 거리
    public int H;       // H : |가로|+|세로| 장애물 무시하여 목표까지의 거리
    public int F { get { return G + H; } }  // F : G + H의 값

    public bool isWall;
    public Node parentNode;

    public Node(bool _isWall, int _x, int _y)   // 생성자
    {
        isWall = _isWall;
        x = _x; y = _y;
    }

}