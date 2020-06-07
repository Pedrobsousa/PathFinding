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

    }

    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < m_width && y >= 0 && y < m_height);
    }
}
