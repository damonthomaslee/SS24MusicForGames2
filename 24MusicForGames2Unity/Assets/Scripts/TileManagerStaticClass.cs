using UnityEngine;

public static class TileManagerStaticClass
{
    public static void DestroyAllTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Object.Destroy(tile);
        }
    }
}