using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight; // ���ϴ�, ���� ���� - ��ü ���� ũ��
    public Vector2Int startPos, targetPos;  // ���� ��ġ, ��ǥ ��ġ
    public Vector2Int endPos;               // ���� ��ǥ ��ġ
    public List<Node> finalNodeList;        // ���� ��� ����Ʈ (�ִܰŸ�)
    public bool allowDiagonal;              // �밢�� �̵� ���
    public bool dontCrossCorner;            // �𼭸� �̵� ����

    int sizeX, sizeY;                       // �־��� ��ǥ�� ũ��
    Node[,] nodeArray;                      // ��ü �� �迭
    Node startNode, targetNode, currentNode, neighborNode;
    List<Node> openList, closedList;        // ���� ����Ʈ�� ���� ����Ʈ


    public void PathFinding()
    {
        sizeX = topRight.x - bottomLeft.x + 1;  // ��ü ���� X��ǥ ũ�� ����
        sizeY = topRight.y - bottomLeft.y + 1;  // ��ü ���� Y��ǥ ũ�� ����
        nodeArray = new Node[sizeX, sizeY];     // ��ü ���� ũ�� ����

        // 2���� �迭(��)�� �ʱ�ȭ - ��ǥ�� ����
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;    // �켱 ��ֹ��� �ƴ��� ���� (���� ��带 �߽����� 8�����)

                // ���� ��带 �߽����� 8����� ��ֹ����� �˻�
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                {
                    if (col.gameObject.layer != LayerMask.NameToLayer("Default"))   // Default�� �ƴ϶��
                    {
                        isWall = true;                                              // ��ֹ��̹Ƿ� ǥ��
                    }
                }

                nodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);     // ��ü ���� ������ ��带 ���� �� ��ü �� ����
            }
        }

        startNode = nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];        // ���� ��ǥ ������ǥ(������ǥ)�� �ƴ�, ��ü �� �迭�� ���� �����ǥ(������ǥ)�� �ʱ�ȭ
        targetNode = nodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];     // ��

        // ���� ����Ʈ�� ���� ��� ����
        openList = new List<Node>() { startNode };  // ���� ����Ʈ:  
        closedList = new List<Node>();              // ���� ����Ʈ:
        finalNodeList = new List<Node>();           // ���� ����Ʈ:


        while (0 < openList.Count)
        {
            // ó���� ���� ����Ʈ�� 0��° ��尡 ���� ���� ������
            currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                // ���� ����Ʈ�� �ð�������� ��带 �о��
                // ���� ����Ʈ�� ��� �� F�� ���� ���� ��带 ���� ���� - ���ٸ� H�� �� ���� ���� ���� ��带 ���� ���� ����
                if (openList[i].F <= currentNode.F && openList[i].H < currentNode.H)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);   // ���� ����Ʈ���� ���� ��� ����
            closedList.Add(currentNode);    // ���� ����Ʈ���� ���� ��� �߰�

            // ���� ��尡 ��ǥ ����� ���
            if (currentNode == targetNode)
            {
                Node targetcurrentNode = targetNode;
                while (targetcurrentNode != startNode)
                {
                    finalNodeList.Add(targetcurrentNode);
                    targetcurrentNode = targetcurrentNode.parentNode;
                }
                finalNodeList.Add(startNode);
                finalNodeList.Reverse();

                //for (int i = 0; i < finalNodeList.Count; i++)
                //{
                //    print(i + "��°�� " + finalNodeList[i].x + ", " + finalNodeList[i].y);
                //}
                return;
            }


            // �֢آע� - �밢�� �̵�
            if (allowDiagonal)
            {
                OpenListAdd(currentNode.x + 1, currentNode.y + 1);
                OpenListAdd(currentNode.x - 1, currentNode.y + 1);
                OpenListAdd(currentNode.x - 1, currentNode.y - 1);
                OpenListAdd(currentNode.x + 1, currentNode.y - 1);
            }

            // �� �� �� �� - ���� �̵�
            OpenListAdd(currentNode.x, currentNode.y + 1);
            OpenListAdd(currentNode.x + 1, currentNode.y);
            OpenListAdd(currentNode.x, currentNode.y - 1);
            OpenListAdd(currentNode.x - 1, currentNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        if (bottomLeft.x <= checkX && checkX < topRight.x + 1 &&                            // ��ü ���� �¿� ������ ����� �ʰ�
            bottomLeft.y <= checkY && checkY < topRight.y + 1 &&                            // ��ü ���� ���� ������ ����� �ʰ�
            !nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall &&              // ��ֹ��� �ƴϸ�
            !closedList.Contains(nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))  // ���� ����Ʈ�� ���ԵǾ� ���� �ʴٸ�
        {
            // �밢�� �̵� ����
            if (allowDiagonal) 
            {
                if (nodeArray[currentNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall &&    // ���� X��ǥ���� ���� ��尡 ��ֹ��̶��, �׸���
                    nodeArray[checkX - bottomLeft.x, currentNode.y - bottomLeft.y].isWall)      // ���� Y��ǥ���� �¿� ��尡 ��ֹ��̶��
                {
                    return;                                                                     // �� ���̷� ��� �� ��
                }
            }

            // �𼭸��� �������� ���� ���� ��
            if (dontCrossCorner)    
            {
                if (nodeArray[currentNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall ||    // ���� X��ǥ���� ���� ��尡 ��ֹ��̶��, �Ǵ�
                    nodeArray[checkX - bottomLeft.x, currentNode.y - bottomLeft.y].isWall)      // ���� Y��ǥ���� �¿� ��尡 ��ֹ��̶��
                {
                    return;                                                                     // �̵� �߿� �������� ��ֹ��� ������ �� ��
                }
            }

            neighborNode = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];                                     // �̵��� ��带 �̿� ��忡 �Ҵ�
            int moveCost = currentNode.G + (currentNode.x - checkX == 0 || currentNode.y - checkY == 0 ? 10 : 14);      // ���� ����� �̵� �Ÿ�(G)�� �̵������ ���Ͽ� -> moveCost�� ����


            // �̵������ �̿����G���� �۰ų�, �Ǵ� ���� ����Ʈ�� ���� �̿� ��尡 ���ٸ� G, H, parentNode�� ���� �� ��������Ʈ�� �߰�
            if (moveCost < neighborNode.G || !openList.Contains(neighborNode))
            {
                neighborNode.G = moveCost;  // �̿� ����� G(�̵� �Ÿ�)�� ����
                neighborNode.H = (Mathf.Abs(neighborNode.x - targetNode.x) + Mathf.Abs(neighborNode.y - targetNode.y)) * 10;    // (���� �������� �̵��Ͽ����� ������ �����ϹǷ� ���ϱ� 10)
                neighborNode.parentNode = currentNode;  // �̿� ����� �θ� ��带 ���� ���� ����

                openList.Add(neighborNode);     // ���� ����Ʈ�� �ش� �̿� ��� �߰�
            }
        }
    }

    void OnDrawGizmos()
    {
        if (finalNodeList.Count != 0)
        {
            for (int i = 0; i < finalNodeList.Count - 1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector2(finalNodeList[i].x, finalNodeList[i].y), new Vector2(finalNodeList[i + 1].x, finalNodeList[i + 1].y));
            }
        }
    }
}