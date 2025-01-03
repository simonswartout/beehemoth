using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    [SerializeField] private HexTileManager hexTileManager;

    private float speed = 5f;
    private Vector3 targetPos;
    private Quaternion targetRotation;

    private void Start()
    {
        // Initialize the target position and rotation
        targetPos = transform.position;
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        // Handle mouse input and update target position if conditions are met
        GetMouseInput();

        // Move towards the target position
        if (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
        else
        {
            // Snap to the target position and rotation when close enough
            transform.position = targetPos;
            transform.rotation = targetRotation;
        }
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hexTileManager.HoveredHexTile != null)
            {
                if (hexTileManager.HoveredHexTile.isWalkable)
                {
                    HexTile tile = hexTileManager.HoveredHexTile;

                    // Use the tile's normal and height to calculate the target position and rotation
                    tile.OrientToTileCenterNormal(gameObject, transform);
                }
            }
        }
    }
}
