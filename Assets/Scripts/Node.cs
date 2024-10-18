using System.Collections;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public Node(bool _walkable, Vector3 _worldpos){
        walkable = _walkable;
        worldPosition = _worldpos;
    }

}
