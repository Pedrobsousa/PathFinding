using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Set up Tile geometry/color properly
public class NodeView : MonoBehaviour
{
    public GameObject tile; //reference to node geometry

    [Range(0, 0.5f)]
    public float borderSize = 0.15f;//Use to trim edges of tile

    //takes node obj to draw Tile
    public void Init(Node node)
    {
        if(tile != null)
        {
            //Helps to see what node the tile corresponds in hierarchy
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
        }
    }

    //Aids visualization
    void ColorNode(Color color, GameObject go)
    {
        if (go != null)
        {
            Renderer goRenderer = go.GetComponent<Renderer>();
            
            if(goRenderer != null)
            {
                goRenderer.material.color = color;
            }
        }
    }

    //Stream line usage
    public void ColorNode(Color color)
    {
        ColorNode(color, tile);
    }

}
