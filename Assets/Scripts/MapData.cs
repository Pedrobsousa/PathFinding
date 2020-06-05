using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    //                          [0,5]...[10,5]
    //Dimensions of Map         [0,0]...[10,0]  [width, height]

    public int height = 5;
    public int width = 10;

    //Returns 2D array with open and blocked Nodes
    public int[,] CreateMap()
    {
        int[,] map = new int[width, height];

        for (int h = 0; h < height; h++)//To loop through height and width [0,0]... [10,0]
        {
            for (int w = 0; w < width; w++)
            {
                map[w, h] = 0; //Defaults NodeType to 0
            }
        }

        //Hard coded blocked Nodes

        map[1, 0] = 1;
        map[8, 0] = 1;

        map[1, 1] = 1;
        map[5, 1] = 1;
        map[8, 1] = 1;

        map[1, 2] = 1;
        map[3, 2] = 1;
        map[4, 2] = 1;
        map[5, 2] = 1;
        map[6, 2] = 1;
        map[8, 2] = 1;

        map[3, 3] = 1;
        map[6, 3] = 1;

        map[3, 4] = 1;
        map[8, 4] = 1;

        return map;
    }
}
