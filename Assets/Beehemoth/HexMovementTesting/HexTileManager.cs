using System.Collections.Generic;
using System.Net.Cache;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class HexTileManager : MonoBehaviour
{
    public List<HexTile> hexTiles = new List<HexTile>();
    public HexTile HoveredHexTile = null;
    
    private void Start()
    {
        //get all hex tiles in the scene
        foreach (HexTile ht in hexTiles)
        {
            if(GetHexTileVertexColor(ht) == Color.blue)
            {
                ht.isWalkable = false;
            }
        }

    }

    private Color GetHexTileVertexColor(HexTile ht)
    {
        Color[] colors = ht.gameObject.GetComponent<MeshFilter>().mesh.colors;
        //return a color based on the vertex color of the hex tile
        return colors[0];
    }

    // public HexTile GetOccupiedHexTile()
    // {
    //     foreach (HexTile ht in hexTiles)
    //     {
    //         if (ht.isOccupied)
    //         {
    //             return ht;
    //         }
    //         else 
    //         {
    //             return null;
    //         }
    //     }

    //     return null;
    // }

    private void Update()
    {
        GetHoveredHexTile();
    }

    private void GetHoveredHexTile()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<HexTile>())
            {
                HoveredHexTile = hit.collider.gameObject.GetComponent<HexTile>();
            }
        }
    }
}
