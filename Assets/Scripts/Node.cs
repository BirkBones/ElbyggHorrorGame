using UnityEngine;
using System.Collections;
using System;
//remember : gcost is cost to get to point. hcost is cost to get to goal. fcost is gcost + fcost
public class Node : IHeapItem<Node> {
    public bool walkable;
    public Vector3 worldPosition;
    public int hcost;
    public int gcost;
    public int xVal; //xval and yval are indices
    public int yVal;
    public Node Parent;
    int heapIndex;
    public Node(bool _walkable, Vector3 _worldpos, int _xval, int _yval){
        walkable = _walkable;
        worldPosition = _worldpos;
        xVal = _xval;
        yVal = _yval;

    }
    public int fCost
    {
        get { return hcost + gcost; }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set{
            heapIndex = value;
        }
    }

    //Implementation of CompareTo method. Gives out a value in [-1,0,1] based on the two nodes f and hcosts. 
    //Result is ; 1 if this node is the most efficient, 0 if both are as efficient, -1 if this is the least efficient.
    public int CompareTo(Node NodeToCompare){
        //This is built-in within Systems: returns 1 if fCost of the current object is greater than the comparison Node, 0 if the fcost is the same, and -1 if the fcost 
        //of the current object is lower
        int compare = fCost.CompareTo(NodeToCompare.fCost);
        if (compare == 0){
            //if the nodes have the same fcost, we want to prioritze the node that has the lowest hcost, so we wont just return 0 immidiately. 
            //So if the current object node's hcost is higher than the comparison node, compare becomes 1. if equal, zero, if hcost is lower, -1.
            compare = hcost.CompareTo(NodeToCompare.hcost);

        }
        //due to the way the code is organized elsewhere, we have to change the sign for it to work.
        return -compare;
    }   
}
