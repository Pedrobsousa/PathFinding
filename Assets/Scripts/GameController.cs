using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;
    public PathFinder pathFinder;
    public int startX = 0;
    public int startY = 0;
    public int goalX = 0;
    public int goalY = 0;


    void Start()
    {
        if(mapData != null && graph != null)
        {
            //Sets up 1s and 0s to describe map
            int[,] mapInstance = mapData.CreateMap();
            graph.Init(mapInstance);
            //Grab graphView script from graph to instantiate tiles
            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            if (graphView != null) 
            {
                graphView.Init(graph);
            }

            //Check with Map and set up the start and goal Nodes for PathFinder.cs
            if(graph.IsWithinBounds(startX, startY) && graph.IsWithinBounds(goalX, goalY) 
               && pathFinder != null)
            {
                Node startNode = graph.nodes[startX, startY];
                Node goalNode = graph.nodes[goalX, goalY];

                pathFinder.Init(graph, graphView, startNode, goalNode);
                //Start Search - add delay here 
                StartCoroutine(pathFinder.SearchRoutine(0.05f));//1/10 of a sec def
            }
            else
            {
                Debug.LogWarning("ESSENTIAL NODES OUT OF BOUNDS");
            }
        }
    }
}
