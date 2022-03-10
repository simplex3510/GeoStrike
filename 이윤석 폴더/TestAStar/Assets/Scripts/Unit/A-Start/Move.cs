using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight; // 좌하단, 우상단 설정 - 전체 맵의 크기
    public Vector2Int startPos, targetPos;  // 시작 위치, 목표 위치
    public Vector2Int endPos;               // 최종 목표 위치
    public List<Node> finalNodeList;        // 최종 노드 리스트 (최단거리)
    public bool allowDiagonal;              // 대각선 이동 허용
    public bool dontCrossCorner;            // 모서리 이동 불허

    int sizeX, sizeY;                       // 주어진 좌표의 크기
    Node[,] nodeArray;                      // 전체 맵 배열
    Node startNode, targetNode, currentNode, neighborNode;
    List<Node> openList, closedList;        // 열린 리스트와 닫힌 리스트


    public void PathFinding()
    {
        sizeX = topRight.x - bottomLeft.x + 1;  // 전체 맵의 X좌표 크기 설정
        sizeY = topRight.y - bottomLeft.y + 1;  // 전체 맵의 Y좌표 크기 설정
        nodeArray = new Node[sizeX, sizeY];     // 전체 맵의 크기 설정

        // 2차원 배열(맵)의 초기화 - 좌표값 설정
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;    // 우선 장애물이 아님을 전제 (현재 노드를 중심으로 8방면이)

                // 현재 노드를 중심으로 8방면이 장애물인지 검사
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                {
                    if (col.gameObject.layer != LayerMask.NameToLayer("Default"))   // Default가 아니라면
                    {
                        isWall = true;                                              // 장애물이므로 표시
                    }
                }

                nodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);     // 전체 맵을 구성할 노드를 생성 후 전체 맵 구성
            }
        }

        startNode = nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];        // 시작 좌표 절대좌표(월드좌표)가 아닌, 전체 맵 배열에 대한 상대좌표(로컬좌표)로 초기화
        targetNode = nodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];     // 상동

        // 열린 리스트에 시작 노드 설정
        openList = new List<Node>() { startNode };  // 열린 리스트:  
        closedList = new List<Node>();              // 닫힌 리스트:
        finalNodeList = new List<Node>();           // 최종 리스트:


        while (0 < openList.Count)
        {
            // 처음엔 오픈 리스트의 0번째 노드가 현재 노드로 설정됨
            currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                // 열린 리스트는 시계방향으로 노드를 읽어옴
                // 열린 리스트의 노드 중 F가 가장 작은 노드를 현재 노드로 - 같다면 H가 더 작은 값을 가진 노드를 현재 노드로 설정
                if (openList[i].F <= currentNode.F && openList[i].H < currentNode.H)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);   // 열린 리스트에서 현재 노드 삭재
            closedList.Add(currentNode);    // 닫힌 리스트에서 현재 노드 추가

            // 현재 노드가 목표 노드일 경우
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
                //    print(i + "번째는 " + finalNodeList[i].x + ", " + finalNodeList[i].y);
                //}
                return;
            }


            // ↗↖↙↘ - 대각선 이동
            if (allowDiagonal)
            {
                OpenListAdd(currentNode.x + 1, currentNode.y + 1);
                OpenListAdd(currentNode.x - 1, currentNode.y + 1);
                OpenListAdd(currentNode.x - 1, currentNode.y - 1);
                OpenListAdd(currentNode.x + 1, currentNode.y - 1);
            }

            // ↑ → ↓ ← - 수평 이동
            OpenListAdd(currentNode.x, currentNode.y + 1);
            OpenListAdd(currentNode.x + 1, currentNode.y);
            OpenListAdd(currentNode.x, currentNode.y - 1);
            OpenListAdd(currentNode.x - 1, currentNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        if (bottomLeft.x <= checkX && checkX < topRight.x + 1 &&                            // 전체 맵의 좌우 범위를 벗어나지 않고
            bottomLeft.y <= checkY && checkY < topRight.y + 1 &&                            // 전체 맵의 상하 범위를 벗어나지 않고
            !nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall &&              // 장애물이 아니며
            !closedList.Contains(nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))  // 닫힌 리스트에 포함되어 있지 않다면
        {
            // 대각선 이동 허용시
            if (allowDiagonal) 
            {
                if (nodeArray[currentNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall &&    // 현재 X좌표에서 상하 노드가 장애물이라면, 그리고
                    nodeArray[checkX - bottomLeft.x, currentNode.y - bottomLeft.y].isWall)      // 현재 Y좌표에서 좌우 노드가 장애물이라면
                {
                    return;                                                                     // 벽 사이로 통과 안 됨
                }
            }

            // 모서리를 가로질러 가지 않을 시
            if (dontCrossCorner)    
            {
                if (nodeArray[currentNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall ||    // 현재 X좌표에서 상하 노드가 장애물이라면, 또는
                    nodeArray[checkX - bottomLeft.x, currentNode.y - bottomLeft.y].isWall)      // 현재 Y좌표에서 좌우 노드가 장애물이라면
                {
                    return;                                                                     // 이동 중에 수직수평 장애물이 있으면 안 됨
                }
            }

            neighborNode = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];                                     // 이동할 노드를 이웃 노드에 할당
            int moveCost = currentNode.G + (currentNode.x - checkX == 0 || currentNode.y - checkY == 0 ? 10 : 14);      // 현재 노드의 이동 거리(G)와 이동비용을 합하여 -> moveCost를 설정


            // 이동비용이 이웃노드G보다 작거나, 또는 열린 리스트에 현재 이웃 노드가 없다면 G, H, parentNode를 설정 후 열린리스트에 추가
            if (moveCost < neighborNode.G || !openList.Contains(neighborNode))
            {
                neighborNode.G = moveCost;  // 이웃 노드의 G(이동 거리)를 설정
                neighborNode.H = (Mathf.Abs(neighborNode.x - targetNode.x) + Mathf.Abs(neighborNode.y - targetNode.y)) * 10;    // (수평 수직으로 이동하였음을 전제로 연산하므로 곱하기 10)
                neighborNode.parentNode = currentNode;  // 이웃 노드의 부모 노드를 현재 노드로 설정

                openList.Add(neighborNode);     // 열린 리스트에 해당 이웃 노드 추가
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