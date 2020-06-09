using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NodeType { Open = 0, Blocked = 1} //Store Different types of Nodes, acessible to all Objects


//It is Necessary to build a Priority Queue for the FrontierNodes so that we can
//Organize them using distance travelled. Highest priority = shortest dist , on the front of the Queue
//CompareTo method way for nodes to compare with one another to establish priority
public class Node : IComparable<Node> //Interface
{

    public NodeType nodeType = NodeType.Open; //By default "walkable"
    public int xIndex = -1, yIndex = -1; //Track position in 2d Array -- set to flag an error if not set (-1 is invalid for arrays)
    public float distanceTravelled = Mathf.Infinity;//No useful value yet
    public Vector3 position; //Keeps track of Nodes position (no transform -- Monobehaviour)
    public int priority;

    public List<Node> neighbors = new List<Node>();

    public Node previous = null; //Indicates best path to Node

    //Constructor method. When we call to build map fill with data
    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    }

    //IComparable interface requirement
    //Determine sorting order of Node for Priority Queue
    public int CompareTo(Node other)
    {
        if(this.priority < other.priority)
        {
            return -1;
        }
        else if(this.priority > other.priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    //Necessary to recalculate path
    public void Reset()
    {
        previous = null;
    }

}
