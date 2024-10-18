using System.Collections;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Transform PlayerPos;
    public Vector2 gridWorldSize; //holds the size of the grid in meters x meters
    public float nodeRadius; // the radius for the nodes, aka half the sidelength of the square
    Node[,] grid; // grid-array

    
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start(){
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid() {
        grid = new Node[gridSizeX,gridSizeY];
        Vector3 worldbottomleft = transform.position - Vector3.right * gridWorldSize.x / 2 
        - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeY; y++){
                Vector3 worldpoint = worldbottomleft + Vector3.right * (x * nodeDiameter + nodeRadius)
                + Vector3.forward * (y*nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldpoint, nodeRadius,unwalkableMask));
                grid[x,y] = new Node(walkable, worldpoint);
            }
        
    }
    }
    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid!=null){
            Node PlayerNode = NodeFromWorldPoint(PlayerPos.position);
            foreach (Node n in grid){
                Gizmos.color = (n.walkable)?(Color.white):(Color.red);
                if (PlayerNode == n){
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * nodeDiameter*0.9f);
        
        }   

    }
}
    // This function might fuck up later when moving the arena
    public Node NodeFromWorldPoint(Vector3 worldpos){
        //WOOPS : here the y coordinate represents the ground, not the height.
        float percX = Mathf.Clamp01(((worldpos.x - transform.position.x) + gridWorldSize.x / 2) / (gridWorldSize.x)); // worldpos.x - transform.x is the local position, that will say this will remain constant even if the arena along with all objects are moved by a distance.
        float percY = Mathf.Clamp01(((worldpos.z - transform.position.z) + gridWorldSize.y / 2) / (gridWorldSize.y));

        int indX = Mathf.RoundToInt(percX * (gridSizeX-1));
        int indY = Mathf.RoundToInt(percY * (gridSizeY-1));
        return grid[indX,indY];
    } 
}