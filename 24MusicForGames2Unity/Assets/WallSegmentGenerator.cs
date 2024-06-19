using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class WallSegmentGenerator : MonoBehaviour
{
    public float height = 10f;
    public float innerRadius = 5f;
    public float outerRadius = 10f;
    public int segments = 36;
    public int numberOfDoors = 1;
    public float doorWidth = 2f;
    public float doorHeight = 3.5f;
    public float overlapAngle = 0.5f; // Increased overlap angle in degrees
    public Material segmentMaterial;

    private GameObject wallSegmentPrefab;
    private GameObject doorSegmentPrefab;

    void Start()
    {
        // Commenting out the automatic generation to ensure it only generates when the button is pressed
        // GenerateWallSegments();
    }

    public void GenerateWallSegments()
    {
        // Destroy the old segments if they exist
        Transform[] existingSegments = transform.GetComponentsInChildren<Transform>();
        foreach (Transform segment in existingSegments)
        {
            if (segment != transform)
            {
                DestroyImmediate(segment.gameObject);
            }
        }

        wallSegmentPrefab = CreateWallSegmentPrefab();
        doorSegmentPrefab = CreateDoorSegmentPrefab();

        float angleStep = 360f / segments;
        float doorAngleStep = 360f / numberOfDoors;

        for (int i = 0; i < segments; i++)
        {
            float angle = angleStep * i;
            float nextAngle = angleStep * (i + 1);

            bool isDoorSegment = false;
            for (int j = 0; j < numberOfDoors; j++)
            {
                float doorAngle = doorAngleStep * j;
                if (angle <= doorAngle && nextAngle > doorAngle)
                {
                    isDoorSegment = true;
                    InstantiateSegment(doorSegmentPrefab, angle);
                    break;
                }
            }

            if (!isDoorSegment)
            {
                InstantiateSegment(wallSegmentPrefab, angle);
            }
        }
    }

    GameObject CreateWallSegmentPrefab()
    {
        GameObject segment = new GameObject("WallSegmentPrefab");
        segment.AddComponent<MeshFilter>();
        segment.AddComponent<MeshRenderer>();
        segment.AddComponent<BoxCollider>(); // Add a box collider

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        float angle = -Mathf.Deg2Rad * overlapAngle / 2;
        float nextAngle = Mathf.Deg2Rad * ((360f / segments) + overlapAngle);

        AddWallSegment(vertices, triangles, uvs, angle, nextAngle, innerRadius, outerRadius, height);

        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };

        mesh.RecalculateNormals();

        MeshFilter meshFilter = segment.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = segment.GetComponent<MeshRenderer>();
        BoxCollider boxCollider = segment.GetComponent<BoxCollider>();

        meshFilter.mesh = mesh;
        meshRenderer.material = segmentMaterial != null ? segmentMaterial : new Material(Shader.Find("Standard"));

        // Configure the box collider
        float midAngle = (angle + nextAngle) / 2;
        float xCenter = Mathf.Cos(midAngle) * (outerRadius + innerRadius) / 2;
        float zCenter = Mathf.Sin(midAngle) * (outerRadius + innerRadius) / 2;
        boxCollider.center = new Vector3(xCenter, height / 2, zCenter);
        boxCollider.size = new Vector3((outerRadius - innerRadius), height, Mathf.Abs(Mathf.Cos(angle) - Mathf.Cos(nextAngle)) * outerRadius);

#if UNITY_EDITOR
        string localPath = "Assets/Prefabs/WallSegmentPrefab.prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAsset(segment, localPath);
#endif

        return segment;
    }

    GameObject CreateDoorSegmentPrefab()
    {
        GameObject segment = new GameObject("DoorSegmentPrefab");
        segment.AddComponent<MeshFilter>();
        segment.AddComponent<MeshRenderer>();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        float angle = -Mathf.Deg2Rad * overlapAngle / 2;
        float nextAngle = Mathf.Deg2Rad * ((360f / segments) + overlapAngle);

        CreateDoorSegment(vertices, triangles, uvs, angle, nextAngle, doorWidth, doorHeight, height);

        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };

        mesh.RecalculateNormals();

        MeshFilter meshFilter = segment.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = segment.GetComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        meshRenderer.material = segmentMaterial != null ? segmentMaterial : new Material(Shader.Find("Standard"));

#if UNITY_EDITOR
        string localPath = "Assets/Prefabs/DoorSegmentPrefab.prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAsset(segment, localPath);
#endif

        return segment;
    }

    void InstantiateSegment(GameObject segmentPrefab, float angle)
    {
        GameObject segment = Instantiate(segmentPrefab, transform);
        segment.transform.localRotation = Quaternion.Euler(0, angle, 0);
        segment.transform.localPosition = Vector3.zero; // Ensure the segments are positioned correctly at the base
    }

    void AddWallSegment(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, float angle, float nextAngle, float innerRadius, float outerRadius, float height)
    {
        int startVertexIndex = vertices.Count;

        // Inner bottom vertices
        vertices.Add(new Vector3(Mathf.Cos(angle) * innerRadius, 0, Mathf.Sin(angle) * innerRadius));
        vertices.Add(new Vector3(Mathf.Cos(nextAngle) * innerRadius, 0, Mathf.Sin(nextAngle) * innerRadius));

        // Inner top vertices
        vertices.Add(new Vector3(Mathf.Cos(angle) * innerRadius, height, Mathf.Sin(angle) * innerRadius));
        vertices.Add(new Vector3(Mathf.Cos(nextAngle) * innerRadius, height, Mathf.Sin(nextAngle) * innerRadius));

        // Outer bottom vertices
        vertices.Add(new Vector3(Mathf.Cos(angle) * outerRadius, 0, Mathf.Sin(angle) * outerRadius));
        vertices.Add(new Vector3(Mathf.Cos(nextAngle) * outerRadius, 0, Mathf.Sin(nextAngle) * outerRadius));

        // Outer top vertices
        vertices.Add(new Vector3(Mathf.Cos(angle) * outerRadius, height, Mathf.Sin(angle) * outerRadius));
        vertices.Add(new Vector3(Mathf.Cos(nextAngle) * outerRadius, height, Mathf.Sin(nextAngle) * outerRadius));

        // UV mapping
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));

        // Triangles for the inner wall
        triangles.Add(startVertexIndex);
        triangles.Add(startVertexIndex + 1);
        triangles.Add(startVertexIndex + 2);

        triangles.Add(startVertexIndex + 1);
        triangles.Add(startVertexIndex + 3);
        triangles.Add(startVertexIndex + 2);

        // Triangles for the outer wall
        triangles.Add(startVertexIndex + 4);
        triangles.Add(startVertexIndex + 5);
        triangles.Add(startVertexIndex + 6);

        triangles.Add(startVertexIndex + 5);
        triangles.Add(startVertexIndex + 7);
        triangles.Add(startVertexIndex + 6);

        // Triangles for the sides
        triangles.Add(startVertexIndex);
        triangles.Add(startVertexIndex + 4);
        triangles.Add(startVertexIndex + 2);

        triangles.Add(startVertexIndex + 4);
        triangles.Add(startVertexIndex + 6);
        triangles.Add(startVertexIndex + 2);

        triangles.Add(startVertexIndex + 1);
        triangles.Add(startVertexIndex + 5);
        triangles.Add(startVertexIndex + 3);

        triangles.Add(startVertexIndex + 5);
        triangles.Add(startVertexIndex + 7);
        triangles.Add(startVertexIndex + 3);
    }

    void CreateDoorSegment(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, float angle, float nextAngle, float doorWidth, float doorHeight, float height)
    {
        int startVertexIndex = vertices.Count;
        float halfWidth = doorWidth / 2f;

        // Center door within the segment
        float doorCenterAngle = (angle + nextAngle) / 2;
        float innerX1 = Mathf.Cos(doorCenterAngle - halfWidth / innerRadius) * innerRadius;
        float innerZ1 = Mathf.Sin(doorCenterAngle - halfWidth / innerRadius) * innerRadius;
float innerX2 = Mathf.Cos(doorCenterAngle + halfWidth / innerRadius) * innerRadius;
float innerZ2 = Mathf.Sin(doorCenterAngle + halfWidth / innerRadius) * innerRadius;    float outerX1 = Mathf.Cos(doorCenterAngle - halfWidth / outerRadius) * outerRadius;
    float outerZ1 = Mathf.Sin(doorCenterAngle - halfWidth / outerRadius) * outerRadius;
    float outerX2 = Mathf.Cos(doorCenterAngle + halfWidth / outerRadius) * outerRadius;
    float outerZ2 = Mathf.Sin(doorCenterAngle + halfWidth / outerRadius) * outerRadius;

    // Bottom vertices (inner and outer)
    vertices.Add(new Vector3(innerX1, 0, innerZ1));
    vertices.Add(new Vector3(innerX2, 0, innerZ2));
    vertices.Add(new Vector3(outerX1, 0, outerZ1));
    vertices.Add(new Vector3(outerX2, 0, outerZ2));

    // Door bottom vertices
    vertices.Add(new Vector3(innerX1, doorHeight, innerZ1));
    vertices.Add(new Vector3(innerX2, doorHeight, innerZ2));
    vertices.Add(new Vector3(outerX1, doorHeight, outerZ1));
    vertices.Add(new Vector3(outerX2, doorHeight, outerZ2));

    // Top vertices (inner and outer)
    vertices.Add(new Vector3(innerX1, height, innerZ1));
    vertices.Add(new Vector3(innerX2, height, innerZ2));
    vertices.Add(new Vector3(outerX1, height, outerZ1));
    vertices.Add(new Vector3(outerX2, height, outerZ2));

    // UV mapping
    uvs.Add(new Vector2(0, 0));
    uvs.Add(new Vector2(1, 0));
    uvs.Add(new Vector2(0, doorHeight / height));
    uvs.Add(new Vector2(1, doorHeight / height));
    uvs.Add(new Vector2(0, doorHeight / height));
    uvs.Add(new Vector2(1, doorHeight / height));
    uvs.Add(new Vector2(0, 1));
    uvs.Add(new Vector2(1, 1));

    // Add triangles for the lower part of the wall segment (below the door)
    triangles.AddRange(new int[]
    {
        startVertexIndex, startVertexIndex + 1, startVertexIndex + 4,
        startVertexIndex + 1, startVertexIndex + 5, startVertexIndex + 4,
        startVertexIndex + 2, startVertexIndex + 3, startVertexIndex + 6,
        startVertexIndex + 3, startVertexIndex + 7, startVertexIndex + 6
    });

    // Add triangles for the upper part of the wall segment (above the door)
    triangles.AddRange(new int[]
    {
        startVertexIndex + 4, startVertexIndex + 5, startVertexIndex + 8,
        startVertexIndex + 5, startVertexIndex + 9, startVertexIndex + 8,
        startVertexIndex + 6, startVertexIndex + 7, startVertexIndex + 10,
        startVertexIndex + 7, startVertexIndex + 11, startVertexIndex + 10
    });
}
#if UNITY_EDITOR
[CustomEditor(typeof(WallSegmentGenerator))]
public class WallSegmentGeneratorEditor : Editor
{
public override void OnInspectorGUI()
{
DrawDefaultInspector();        WallSegmentGenerator script = (WallSegmentGenerator)target;
        if (GUILayout.Button("Generate Wall Segments"))
        {
            script.GenerateWallSegments();
        }
    }
}
#endif
}