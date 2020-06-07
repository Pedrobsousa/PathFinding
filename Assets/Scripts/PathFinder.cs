using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    Node m_startNode, m_goalNode;
    Graph m_graph;
    GraphView m_graphView;

    Queue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;//After frontier nodes
    List<Node> m_pathNodes;//path to goal nodes

    #region Node Colors
    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.blue;
    #endregion

    public bool isComplete = false;//if the search is done
    int m_iterations = 0;//iterations used to explore

    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        if (graph == null || graphView == null || start == null || goal == null)
        {
            Debug.LogWarning("PATHFINDER INIT error: Missing Component(s)");
            return;

        }

        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER INIT error: Invalid Start/Goal Node");
            return;
        }

        //Cache params
        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_goalNode = goal;


        DisplayColors(graphView, start, goal);

        m_frontierNodes = new Queue<Node>();
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();
        //Clear previous Node to null / previous runs of search
        for (int x = 0; x < m_graph.Width; x++)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                m_graph.nodes[x, y].Reset();
            }
        }

        //Reset
        isComplete = false;
        m_iterations = 0;
    }

    //Diagnostic Tool - Paints Special Nodes

    void DisplayColors()
    {
        DisplayColors(m_graphView, m_startNode, m_goalNode);
    }

    void DisplayColors(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            return;
        }

        if (m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        if (m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes, exploredColor);
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }

    //Set up Delay for search

    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        yield return null;

        while (!isComplete)
        {
            if(m_frontierNodes.Count > 0)//Make sure we have a Node in Queue
            {
                Node currentNode = m_frontierNodes.Dequeue();
                m_iterations++;
                //Transfer Node to explored List
                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                ExpandFrontier(currentNode);
                DisplayColors();

                yield return new WaitForSeconds(timeStep);
            }
            else//No more Frontier Nodes
            {
                Debug.LogWarning("DONE SEARCH");
                isComplete = true;
            }
        }
    }

    //Add Neighbors to Frontier Queue of node and set up trail back
    void ExpandFrontier(Node node)
    {
        if(node != null)
        {
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                if(!m_exploredNodes.Contains(node.neighbors[i]) 
                   && !m_frontierNodes.Contains(node.neighbors[i]))
                {
                    //BreadCrumb Trail to previous path of nodes
                    node.neighbors[i].previous = node;
                    m_frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }
}
