using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PathFinding : MonoBehaviour
{
    public Grid grid;
    public Transform Enemy;
    public Transform Player;
    void awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        FindPath(Enemy.position, Player.position);
    }
    void FindPath(Vector3 startPos, Vector3 TargetPos)
    {
        Node StartNode = grid.NodeFromWorldPoint(startPos);
        Node TargetNode = grid.NodeFromWorldPoint(TargetPos);


        List<Node> OpenSet = new List<Node>();
        HashSet<Node> ClosedSet = new HashSet<Node>();
        OpenSet.Add(StartNode);

        while (OpenSet.Count > 0)
        {
            Node CurrentNode = OpenSet[0];
            for (int i = 1; i < OpenSet.Count; i++) {
                if (OpenSet[i].fCost < CurrentNode.fCost || OpenSet[i].fCost == CurrentNode.fCost && OpenSet[i].hcost < CurrentNode.hcost) {
                    CurrentNode = OpenSet[i];
                }
            }
            OpenSet.Remove(CurrentNode);
            ClosedSet.Add(CurrentNode);
            if (CurrentNode == TargetNode)
            {
                RetracePath(StartNode, TargetNode);
                return;
            }
            foreach (Node n in grid.GetNeighbours(CurrentNode)) { 
                if (!n.walkable || ClosedSet.Contains(n)) //n is the neighbour node
                {
                    continue;
                }
                int NewGCost = CurrentNode.gcost + GetDistance(CurrentNode, n);
                if (NewGCost < n.gcost || !OpenSet.Contains(n))
                {
                    n.gcost = NewGCost;
                    n.hcost = GetDistance(n, TargetNode);
                    n.Parent = CurrentNode;
                    if (!OpenSet.Contains(n))
                    {
                        OpenSet.Add(n);
                    }
                }

            }
        }
    }

    void RetracePath(Node StartNode, Node EndNode)
    {
        List<Node> path = new List<Node>();
        Node CurrentNode = EndNode;
        while (CurrentNode != StartNode) {
            path.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }
        path.Reverse();
        grid.path = path;

    }
    int GetDistance(Node start, Node End)
    {
        int dstX = Mathf.Abs(start.xVal - End.xVal);
        int dstY = Mathf.Abs(start.yVal - End.yVal);

        if (dstX > dstY)
        {
            return dstY * 14 + (dstX - dstY) * 10;
        }
        return dstX * (14) + (dstY - dstX) * 10;
    }
}
