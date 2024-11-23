using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;



public class PathFinding : MonoBehaviour
{
    public Grid grid;
    public Transform Enemy;
    public Transform Player;
    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")){
            FindPath(Enemy.position, Player.position);
        }
    }
    void FindPath(Vector3 startPos, Vector3 TargetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node StartNode = grid.NodeFromWorldPoint(startPos);
        Node TargetNode = grid.NodeFromWorldPoint(TargetPos);

        // The heap has to hold, at max, gridX * gridY elements.
        Heap<Node> OpenSet = new Heap<Node>(grid.MaxSize);
        // a hashset is just an unordered set (the c# set variant closest to the mathematical interpretation)
        HashSet<Node> ClosedSet = new HashSet<Node>();
        //Naturally we want to expand the openset based on the first open node.
        OpenSet.Add(StartNode);

        //As long as there are nodes in the openset (nodes that havent been looked at yet), we want to look through them. (unless we have found a short path)
        while (OpenSet.Count > 0)
        {
            //Selects the node with the lowest fcost (and or lowest hcost) while also removing the node from the set. The heap is structured like a min graph
            // where the nodes that have the lowest fcost (and possibly hcost) are placed at the top. This is the reason that the function is called removefirst,
            //because the selected node will always be the first node of the heap.
            Node CurrentNode = OpenSet.RemoveFirst();
            //Since we will have looked at the currentnode, we have to add it to the closedset.
            ClosedSet.Add(CurrentNode);
            if (CurrentNode == TargetNode)
            {
                sw.Stop();
                print("Path Found : " + sw.ElapsedMilliseconds + " ms");
                RetracePath(StartNode, TargetNode);
                return;
            }
            foreach (Node n in grid.GetNeighbours(CurrentNode)) { 
                if (!n.walkable || ClosedSet.Contains(n)) // n is the neighbour node. We dont want to iterate over the node if we already have (if we already have, then the node will be in ClosedSet) or if it cant be walked on anyway.
                {
                    continue; // skips to the next iteration.
                }
                int NewGCost = CurrentNode.gcost + GetDistance(CurrentNode, n); // If the node has not been iterated over before, then we will recaculate the g cost since it might be smaller
                if (NewGCost < n.gcost || !OpenSet.Contains(n)) //You want to do whats below both if the gcost has not been defined before, but also if it is lower.
                {
                    n.gcost = NewGCost;
                    n.hcost = GetDistance(n, TargetNode); //this could be moved to be below the next if statement.
                    n.Parent = CurrentNode; // want the node with the shortest path to n to be the parent.
                    if (!OpenSet.Contains(n))
                    {
                        OpenSet.Add(n); // if the node has not yet been iterated over, we want to iterate over it by adding it to the openset.
                        // Note that the same also goes even for closed nodes, since if the g cost is smaller, then you will want to update the gcost of the children notes too.
                        
                    }
                } // in short, this makes sure that a g cost is already updated to be the smallest value we know of. After checking all the nodes of a specific node, it runs the while loop again. Then it iterates over all the nodes, and
                // chooses to look further on the one that has the lowest f and h cost. If current node is the end, we will return, because now we know that all nodes relevant for the path, have been given a parent, so that one can retrace
                // the steps to the end point, since each nodes parent is the node that has the shortest path to it. 

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
        return dstX * 14 + (dstY - dstX) * 10;
    }
}


// useful information:
// hcost = heuristic cost left to goal. gcost = cost to get to the node. fcost = g + h
// openset: contains all nodes that havent been looked through yet
// closedset : contains all nodes that has been looked through, or is blocked by an obstacle. 



//Node CurrentNode = OpenSet.RemoveFirst(); replaces all of the code below.
// the script iterates over every single node to see which node in the openset has the lowest fcost and should thus be looked more closely at.
// (remember that even though we havent looked directly at the open nodes, we have marked them indirectly with fcosts.)
// by implementing a heap we are able to do this comparison much faster due to the structure of graphs.

// for (int i = 1; i < OpenSet.Count; i++) {
//                 if (OpenSet[i].fCost < CurrentNode.fCost || OpenSet[i].fCost == CurrentNode.fCost && OpenSet[i].hcost < CurrentNode.hcost) {
//                     CurrentNode = OpenSet[i];
//                 }
//             }
//             OpenSet.Remove(CurrentNode); //removes the relevant node from the openset.