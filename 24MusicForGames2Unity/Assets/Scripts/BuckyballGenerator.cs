using UnityEngine;

public class BuckyballGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // Prefab of the tile
    public float radius = 100f; // Radius of the Buckyball, 200m diameter

    void Start()
    {
        GenerateBuckyball();
    }

    void GenerateBuckyball()
    {
        // Constants for the golden ratio
        float phi = (1 + Mathf.Sqrt(5)) / 2;

        // Create the 12 vertices of an icosahedron
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1,  phi,  0),
            new Vector3( 1,  phi,  0),
            new Vector3(-1, -phi,  0),
            new Vector3( 1, -phi,  0),

            new Vector3( 0, -1,  phi),
            new Vector3( 0,  1,  phi),
            new Vector3( 0, -1, -phi),
            new Vector3( 0,  1, -phi),

            new Vector3( phi,  0, -1),
            new Vector3( phi,  0,  1),
            new Vector3(-phi,  0, -1),
            new Vector3(-phi,  0,  1)
        };

        // Normalize to unit sphere
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i].normalized * radius;
        }

        // Create the 20 faces of the icosahedron
        int[,] faces = new int[,]
        {
            {0, 11, 5}, {0, 5, 1}, {0, 1, 7}, {0, 7, 10}, {0, 10, 11},
            {1, 5, 9}, {5, 11, 4}, {11, 10, 2}, {10, 7, 6}, {7, 1, 8},
            {3, 9, 4}, {3, 4, 2}, {3, 2, 6}, {3, 6, 8}, {3, 8, 9},
            {4, 9, 5}, {2, 4, 11}, {6, 2, 10}, {8, 6, 7}, {9, 8, 1}
        };

        // Create tiles at each vertex
        foreach (var vertex in vertices)
        {
            GameObject tile = Instantiate(tilePrefab, vertex, Quaternion.identity);
            tile.tag = "Tile"; // Ensure the tile has the "Tile" tag

            // Add a Rigidbody component if not already present
            if (!tile.GetComponent<Rigidbody>())
            {
                tile.AddComponent<Rigidbody>();
            }
        }

        // Create faces for visual reference (optional)
        for (int i = 0; i < faces.GetLength(0); i++)
        {
            Vector3 v1 = vertices[faces[i, 0]];
            Vector3 v2 = vertices[faces[i, 1]];
            Vector3 v3 = vertices[faces[i, 2]];

            CreateFace(v1, v2, v3);
        }
    }

    void CreateFace(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        GameObject face = new GameObject("Face");
        face.transform.parent = transform;

        Mesh mesh = new Mesh();
        face.AddComponent<MeshFilter>().mesh = mesh;
        face.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

        mesh.vertices = new Vector3[] { v1, v2, v3 };
        mesh.triangles = new int[] { 0, 1, 2 };
        mesh.RecalculateNormals();
    }
}
