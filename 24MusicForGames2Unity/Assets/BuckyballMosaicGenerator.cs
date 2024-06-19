using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BuckyballMosaicGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // Prefab of the tile
    public float radius = 100f; // Radius of the Buckyball, 200m diameter
    public int numberOfTiles = 10000; // Total number of tiles
    public float floorY = 0f; // Y position of the floor

    private List<GameObject> spawnedTiles = new List<GameObject>();

    public void GenerateMosaicBuckyball()
    {
        ClearTiles();

        int generatedTiles = 0;
        while (generatedTiles < numberOfTiles)
        {
            Vector3 pointOnSphere = Random.onUnitSphere * radius;
            if (pointOnSphere.y >= floorY) // Only use points above the floor
            {
                GameObject tile = Instantiate(tilePrefab, pointOnSphere, Quaternion.identity, transform);
                tile.tag = "Tile"; // Ensure the tile has the "Tile" tag

                // Align the tile with the surface of the sphere
                Vector3 directionFromCenter = (tile.transform.position - transform.position).normalized;
                tile.transform.rotation = Quaternion.FromToRotation(Vector3.up, directionFromCenter);
                
                // Rotate 90 degrees around the local x-axis to lay flat
                tile.transform.Rotate(90f, 0f, 0f);

                // Add a Rigidbody component if not already present
                if (!tile.GetComponent<Rigidbody>())
                {
                    tile.AddComponent<Rigidbody>();
                }

                spawnedTiles.Add(tile);
                generatedTiles++;
            }
        }
    }

    public void ClearTiles()
    {
        foreach (GameObject tile in spawnedTiles)
        {
            if (tile != null)
            {
                DestroyImmediate(tile);
            }
        }
        spawnedTiles.Clear();
    }

    void OnDrawGizmosSelected()
    {
        // Draw the sphere radius in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
