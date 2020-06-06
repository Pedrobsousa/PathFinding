using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapData : MonoBehaviour
{
    //                          [0,5]...[10,5]
    //Dimensions of Map         [0,0]...[10,0]  [width, height]

    public int height = 5;
    public int width = 10;

    //Update to use Resources methods for Unity
    public TextAsset textAsset;

    #region #!!TEXTPARSING!!
    //Parse text file delimited by new lines while preserving blank lines
    public List<string> GetTextFromFile(TextAsset tAsset)
    {
        List<string> lines = new List<string>();

        if(tAsset != null)
        {
            string textData = tAsset.text;
            string[] delimiters = { "\r\n", "\n" };//For Windows and Unix/MacOS
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();//Required to match our Grid System other wise its read top to bottom
        }
        else
        {
            Debug.LogWarning("MAPDATA GeTextFromFile error: Invalid textfile");
        }

        return lines;
    }

    //Overloaded version
    public List<string> GetTextFromFile()
    {
        return GetTextFromFile(textAsset);
    }
    #endregion

    //Set dimensions according to getTextFromFile method in #TEXTPARSING#
    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;
        //Get longest width from file to make sure we account for blank space if any !uniform file
        foreach (string line in textLines)
        {
            if(line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    //Returns 2D array with open and blocked Nodes
    public int[,] CreateMap()
    {
        List<string> lines = new List<string>();
        lines = GetTextFromFile();//As long as we have a valid textAsset ref
        SetDimensions(lines);

        int[,] map = new int[width, height];

        for (int h = 0; h < height; h++)//To loop through height and width [0,0]... [10,0]
        {
            for (int w = 0; w < width; w++)
            {
                //avoid index out of range
                if (lines[h].Length > w)
                {
                    map[w, h] = (int)Char.GetNumericValue(lines[h][w]);//Needs to be reversed to get horizontal line(h) first
                }

            }
        }

        ////Hard coded blocked Nodes

        //map[1, 0] = 1;
        //map[8, 0] = 1;

        //map[1, 1] = 1;
        //map[5, 1] = 1;
        //map[8, 1] = 1;

        //map[1, 2] = 1;
        //map[3, 2] = 1;
        //map[4, 2] = 1;
        //map[5, 2] = 1;
        //map[6, 2] = 1;
        //map[8, 2] = 1;

        //map[3, 3] = 1;
        //map[6, 3] = 1;

        //map[3, 4] = 1;
        //map[8, 4] = 1;

        return map;
    }
}
