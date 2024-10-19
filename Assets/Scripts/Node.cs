using System.Collections;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int hcost;
    public int gcost;
    public int xVal;
    public int yVal;
    public Node Parent;

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

}
