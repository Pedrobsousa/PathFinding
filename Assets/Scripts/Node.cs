using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NodeType { Open = 0, Blocked = 1} //Store Different types of Nodes, acessible to all Objects

public class Node
{

    public NodeType nodeType = NodeType.Open; //By default "walkable"
    public int xIndex = -1, yIndex = -1; //Track position in 2d Array -- set to flag an error if not set (-1 is invalid for arrays)
    public float distanceTravelled = Mathf.Infinity;//No useful value yet
    public Vector3 position; //Keeps track of Nodes position (no transform -- Monobehaviour)

    public List<Node> neighbors = new List<Node>();

    public Node previous = null; //Indicates best path to Node

    //Constructor method. When we call to build map fill with data
    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    } 

    //Necessary to recalculate path
    public void Reset()
    {
        previous = null;
    }

}
