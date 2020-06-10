using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    Node m_startNode, m_goalNode;
    Graph m_graph;
    GraphView m_graphView;

    PriorityQueue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;//After frontier nodes
    List<Node> m_pathNodes;//path to goal nodes

    #region Node Colors
    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.blue;
    #endregion

    public bool exitOnGoal = true;
    public bool isComplete = false;//if the search is done

    public bool showIterations = false;//toggle diagnostics
    public bool showArrows = true;
    public bool showColors = true;

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
        //Replace with basic Method to color start and goal
        DisplayColors(graphView, start, goal);

        m_frontierNodes = new PriorityQueue<Node>();
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
        m_startNode.distanceTravelled = 0;//Replace infinity with 0 as we explore
    }


    //Set up search with optional delay
    public IEnumerator SearchRoutine(float timeStep = 0f)
    {
        yield return null;
        while (!isComplete)
        {
            if (m_frontierNodes.Count > 0)//Make sure we have a Node in Queue
            {
                Node currentNode = m_frontierNodes.Dequeue();
                //Transfer Node to explored List
                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                //Dijkstra
                //ExpandFrontierDijkstra(currentNode);
                ExpandFrontierAstar(currentNode);
                //When we find the goalNode get path
                if (m_frontierNodes.Contains(m_goalNode))
                {
                    //Debug.LogError("We found goal Node");
                    m_pathNodes = GetPathNodes(m_goalNode);
                    if (exitOnGoal) {
                        ShowDiagnostics();
                        isComplete = true; 
                    }
                }
                //show diagnostics (colors / arrows)
                if (showIterations)
                {
                    ShowDiagnostics();
                    yield return new WaitForSeconds(timeStep);
                }
            }
            else//No more Frontier Nodes
            {
                Debug.LogWarning("DONE SEARCH");
                isComplete = true;
            }
        }
        ShowDiagnostics();
    }

    //Get List of Path Node
    List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();

        if(endNode == null)
        {
            return path;
        }
        //Start by adding "goal node"
        path.Add(endNode);
        Node currentNode = endNode.previous;//Access previous Node for trail
        //Travel backward through Graph
        while(currentNode != null)
        {
            //Insert used instead to add node to front
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }
        //Debug.Log("Returning path");
        return path;
    }

    //Based on Dijkstra's algorithm
    void ExpandFrontierDijkstra(Node node)
    {
        if(node != null)
        {
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                if(!m_exploredNodes.Contains(node.neighbors[i]))
                {
                    //Distance between neighbor of the node                                         !!!
                    float distanceToNeighbor = m_graph.GetNodeDistance(node, node.neighbors[i]);//Add move nodetype as move cost
                    float newDistanceTravelled = distanceToNeighbor + node.distanceTravelled; //+ (int)node.nodeType;

                    //Only if never travelled to(still has Infinity as distanceTravelled)
                    //or the new distance travelled is shorter than the existing one in neighbor
                    //Only re route the previous node to current if we found a shorter path
                    if (float.IsPositiveInfinity(node.neighbors[i].distanceTravelled) ||
                        newDistanceTravelled < node.neighbors[i].distanceTravelled) 
                    {
                        node.neighbors[i].previous = node;
                        node.neighbors[i].distanceTravelled = newDistanceTravelled;
                       
                    }

                    if (!m_frontierNodes.Contains(node.neighbors[i]))
                    {
                        node.neighbors[i].priority = node.neighbors[i].distanceTravelled;
                        m_frontierNodes.Enqueue(node.neighbors[i]);
                    }
                        
                }
            }
        }
    }

    //Much like the Dij's we find the shortest path but 
    //We improve the priority(Frontier Nodes) part of the algorithm 
    //Using the heuristic function:
    //F(node.priority) = G(.distanceTravelled) + H(m_graph.GetNodeDistance(node.neighbors[i], m_goalNode))
    void ExpandFrontierAstar(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                if (!m_exploredNodes.Contains(node.neighbors[i]))
                {
                    //Distance between neighbor of the node                                         !!!
                    float distanceToNeighbor = m_graph.GetNodeDistance(node, node.neighbors[i]);//Add move nodetype as move cost
                    float newDistanceTravelled = distanceToNeighbor + node.distanceTravelled; //+ (int)node.nodeType;

                    //Only if never travelled to(still has Infinity as distanceTravelled)
                    //or the new distance travelled is shorter than the existing one in neighbor
                    //Only re route the previous node to current if we found a shorter path
                    if (float.IsPositiveInfinity(node.neighbors[i].distanceTravelled) ||
                        newDistanceTravelled < node.neighbors[i].distanceTravelled)
                    {
                        node.neighbors[i].previous = node;
                        node.neighbors[i].distanceTravelled = newDistanceTravelled;

                    }

                    if (!m_frontierNodes.Contains(node.neighbors[i]) && m_graph != null)
                    {
                        //This functions as our H
                        float distanceToGoal = m_graph.GetNodeDistance(node.neighbors[i], m_goalNode);

                        node.neighbors[i].priority = node.neighbors[i].distanceTravelled + distanceToGoal;
                        m_frontierNodes.Enqueue(node.neighbors[i]);
                    }

                }
            }
        }
    }





    //Diagnostic Tool - Paints Special Nodes
    void ShowDiagnostics()
    {
        if (showColors)
        {
            DisplayColors();
            //Debug.Log("Diagnostics");
        }

        if (m_graphView != null && showArrows)
        {
            m_graphView.ShowNodeArrows(m_frontierNodes.ToList());
            if (m_frontierNodes.Contains(m_goalNode))
            {
                m_graphView.ShowNodeArrows(m_pathNodes);
            }
            
        }
    }

    void DisplayColors()
    {
        DisplayColors(m_graphView, m_startNode, m_goalNode);
    }

    void DisplayColors(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            Debug.Log("DisplayColors returned");
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

        //Color Path 
        if (m_pathNodes != null && m_pathNodes.Count > 0)
        {
            Debug.Log("Coloring Path");
            graphView.ColorNodes(m_pathNodes, pathColor);

        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];
        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];
        if (startNodeView != null && goalNodeView != null)
        {
            startNodeView.ColorNode(startColor);
            goalNodeView.ColorNode(goalColor);
        }
    }


}
