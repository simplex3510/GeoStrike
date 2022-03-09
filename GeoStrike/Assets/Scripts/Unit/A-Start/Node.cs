using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public int x, y;    // ����� ��ǥ��  
    public int G;       // G : �������κ��� �̵��ߴ� �Ÿ�
    public int H;       // H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�
    public int F { get { return G + H; } }  // F : G + H�� ��

    public bool isWall;
    public Node parentNode;

    public Node(bool _isWall, int _x, int _y)   // ������
    {
        isWall = _isWall;
        x = _x; y = _y;
    }

}