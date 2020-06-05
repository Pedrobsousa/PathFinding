using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;

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
        }
    }
}
