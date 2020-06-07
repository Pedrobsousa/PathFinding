using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Translate 1s and 0s of MapData into Nodes[]
//MapData assigns the info 
public class Graph : MonoBehaviour
{
    //corresponds to ints from MapData
    public Node[,] nodes;
    //keeps track of 0s or blocked Nodes
    public List<Node> walls = new List<Node>();

    //To keep track of data, easier to read
    int[,] m_mapData;
    int m_width;
    int m_height;
    public int Height { get { return m_height; } }//Expose map dimensions
    public int Width { get { return m_width; } }//Read only

    public static readonly Vector2[] allDirections =
{
        new Vector2(0f,1f),
        new Vector2(1f,1f),
        new Vector2(1f,0f),
        new Vector2(1f,-1f),
        new Vector2(0f,-1f),
        new Vector2(-1f,-1f),
        new Vector2(-1f,0f),
        new Vector2(-1f,1f)
    };

    //Run only once for each Graph
    public void Init(int[,] mapData)
    {
        m_mapData = mapData;
        m_width = mapData.GetLength(0);//We get a 2D array, this will
        m_height = mapData.GetLength(1);//make sure we get the right dimensions


        nodes = new Node[m_width, m_height];

        //Generate Nodes based on mapData
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                //Cast the 1 or 0 as a NodeType
                NodeType type = (NodeType)mapData[x, y];//[0,0] returns 0 so we Cast it as type
                Node newNode = new Node(x, y, type);
                nodes[x, y] = newNode;

                //We want to lay nodes along x and z plane
                newNode.position = new Vector3(x, 0, y);

                if(type == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }
            }
        }
        //Set up nodes list with neighbors
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                if (nodes[x, y].nodeType != NodeType.Blocked)
                {
                    nodes[x, y].neighbors = GetNeighbors(x, y);
                }
            }
        }

    }

    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < m_width && y >= 0 && y < m_height);
    }

    List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighborNodes = new List<Node>();

        foreach (Vector2 dir in directions)
        {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;

            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null &&
                nodeArray[newX, newY].nodeType != NodeType.Blocked)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }
        return neighborNodes;

    }

    List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, nodes, allDirections);
    }
}
