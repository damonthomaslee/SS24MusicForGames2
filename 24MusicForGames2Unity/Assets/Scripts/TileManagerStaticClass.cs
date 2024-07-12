using UnityEngine;

public static class TileManagerStaticClass
{
    public static void DestroyAllTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Debug.Log("Destroying tile: " + tile.name + " with tag: " + tile.tag);
            Object.Destroy(tile);
        }
    }

    public static void DestroyTile(GameObject tile)
    {
        if (tile == null)
        {
            Debug.LogError("Tile is null and cannot be destroyed.");
            return;
        }

        if (!tile.CompareTag("Tile"))
        {
            Debug.LogError("Tile does not have the correct tag: " + tile.tag);
            return;
        }

        Debug.Log("Destroying tile: " + tile.name + " with tag: " + tile.tag);
        Object.Destroy(tile);
    }
}
