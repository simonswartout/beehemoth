using Unity.VisualScripting;
using UnityEngine;
using UnityHexPlanet;

public class HexTile : MonoBehaviour
{
    public bool isWalkable = true;
    public bool isHighlighted = false;
    [SerializeField] HexChunkRenderer hexChunkRenderer;
    [SerializeField] private Transform snapPos;

    private void OnValidate()
    {
        if (hexChunkRenderer == null)
            hexChunkRenderer = GetComponent<HexChunkRenderer>(); 
    }

    private void Awake()
    {
        if(snapPos == null)
        {
            Mesh reference = hexChunkRenderer.GetHexChunk().GetMesh();
            //create a new gameobject at the top extent of the mesh
            snapPos = new GameObject("SnapPos").transform;
            snapPos.SetParent(transform);
            snapPos.localPosition = new Vector3(0, reference.bounds.extents.y, 0);                                                                                          
        }
    }

    public Vector3 GetOrigin()
    {
        // Calculate the position of the tile's center using the chunk's origin
        return hexChunkRenderer.GetHexChunk().origin;
    }

    public Vector3 GetNormalDirection()
    {
        // Calculate the normal using the planet's center and this tile's position

        // Normal is the vector from the planet's center to the tile's origin
        Vector3 normal = (GetOrigin() - Vector3.zero).normalized;
        return normal;
    }

    public float GetHeight()
    {
        // Return the distance from the planet's center to the tile's center
        return Vector3.Distance(GetOrigin(), Vector3.zero);
    }

    public Transform OrientToTileCenterNormal(GameObject obj, Transform transform)
    {
        // Orient the transform to the tile's normal direction and center
        obj.transform.position = GetOrigin();
        obj.transform.rotation = Quaternion.FromToRotation(transform.up, GetNormalDirection()) * transform.rotation;
        return transform;
    }
}
