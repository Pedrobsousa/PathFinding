using System.Collections;
using System.Collections.Generic;
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

    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        if(graph == null || graphView == null || start == null || goal == null)
        {
            Debug.LogWarning("PATHFINDER INIT error: Missing Component(s)");
            return;

        }

        if(start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER INIT error: Invalid Start/Goal Node");
            return;
        }

        //Cache params
        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_goalNode = goal;
        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];
        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];
        //Access nodeViews array, get ref, color nodes
        if(startNodeView != null && goalNodeView != null)
        {
            startNodeView.ColorNode(startColor);
            goalNodeView.ColorNode(goalColor);
        }

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
    }

}
