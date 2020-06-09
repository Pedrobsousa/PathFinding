using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Instatiates the actual Nodes from nodeview.cs
[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;//Identify nodeViews to color
    public Color baseColor = Color.white;
    public Color wallColor = Color.black;

    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("GRAPHVIEW No Graph to initialize");
            return;
        }

        nodeViews = new NodeView[graph.Width, graph.Height];

        //In graph we create nodes[] based on the MapData
        foreach (Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();//from prefab get Nodeview ref

            if (nodeView != null)
            {
                nodeView.Init(n);
                //Allows to see each Node => NodeView
                nodeViews[n.xIndex, n.yIndex] = nodeView;//Add NodeView to Array using n
                if (n.nodeType == NodeType.Blocked)
                {
                    nodeView.ColorNode(wallColor);
                }
                else
                {
                    nodeView.ColorNode(baseColor);
                }
            }

        }
    }


    //Color multiple nodes at once
    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];

                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }

    public void ShowNodeArrows(Node node)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];
            if (nodeView != null)
            {
                nodeView.ShowArrow();
            }
        }
    }
    //Overloaded takes List<Node>
    public void ShowNodeArrows(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            ShowNodeArrows(n);
        }
    }
    

}
