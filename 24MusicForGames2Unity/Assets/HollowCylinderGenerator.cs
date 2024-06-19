using UnityEngine;
using System.Collections.Generic; // Add this directive

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class HollowCylinderGenerator : MonoBehaviour
{
    public float height = 10f;
    public float innerRadius = 5f;
    public float outerRadius = 10f;
    public int segments = 36;
    public int numberOfDoors = 1;
    public float doorWidth = 2f;
    public float doorHeight = 4f;
    public Material cylinderMaterial;

    private GameObject hollowCylinder;

    void Start()
    {
        // Commenting out the automatic generation to ensure it only generates when the button is pressed
        // GenerateHollowCylinder();
    }

    public void GenerateHollowCylinder()
    {
        // Destroy the old cylinder if it exists
        Transform existingCylinder = transform.Find("HollowCylinder");
        if (existingCylinder != null)
        {
            DestroyImmediate(existingCylinder.gameObject);
        }

        hollowCylinder = new GameObject("HollowCylinder");
        hollowCylinder.transform.SetParent(transform);
        hollowCylinder.AddComponent<MeshFilter>();
        hollowCylinder.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = hollowCylinder.AddComponent<MeshCollider>();
        Rigidbody rb = hollowCylinder.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        MeshFilter meshFilter = hollowCylinder.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = hollowCylinder.GetComponent<MeshRenderer>();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        float angleStep = 360f / segments;
        float doorAngleStep = 360f / numberOfDoors;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            float nextAngle = Mathf.Deg2Rad * angleStep * (i + 1);

            // Check if this segment contains a door
            bool isDoorSegment = false;
            for (int j = 0; j < numberOfDoors; j++)
            {
                float doorAngle = Mathf.Deg2Rad * doorAngleStep * j;
                if (angle <= doorAngle && nextAngle > doorAngle)
                {
                    isDoorSegment = true;
                    CreateDoor(vertices, triangles, angle, nextAngle, doorAngle, doorWidth, doorHeight, height);
                    break;
                }
            }

            if (!isDoorSegment)
            {
                // Create vertices and triangles for the outer wall
                AddWallSegment(vertices, triangles, angle, nextAngle, outerRadius, height);

                // Create vertices and triangles for the inner wall
                AddWallSegment(vertices, triangles, angle, nextAngle, innerRadius, height);
            }
        }

        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.material = cylinderMaterial != null ? cylinderMaterial : new Material(Shader.Find("Standard"));
    }

    void AddWallSegment(List<Vector3> vertices, List<int> triangles, float angle, float nextAngle, float radius, float height)
    {
        int startVertexIndex = vertices.Count;

        // Bottom vertices
        vertices.Add(new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius));
        vertices.Add(new Vector3(Mathf.Cos(nextAngle) * radius, 0, Mathf.Sin(nextAngle) * radius));

        // Top vertices
        vertices.Add(new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius));
        vertices.Add(new Vector3(Mathf.Cos(nextAngle) * radius, height, Mathf.Sin(nextAngle) * radius));

        // Triangles
        triangles.Add(startVertexIndex);
        triangles.Add(startVertexIndex + 2);
        triangles.Add(startVertexIndex + 1);

        triangles.Add(startVertexIndex + 2);
        triangles.Add(startVertexIndex + 3);
        triangles.Add(startVertexIndex + 1);
    }

    void CreateDoor(List<Vector3> vertices, List<int> triangles, float angle, float nextAngle, float doorAngle, float doorWidth, float doorHeight, float cylinderHeight)
    {
        int startVertexIndex = vertices.Count;
        float halfWidth = doorWidth / 2f;

        float innerX1 = Mathf.Cos(doorAngle - halfWidth / innerRadius) * innerRadius;
        float innerZ1 = Mathf.Sin(doorAngle - halfWidth / innerRadius) * innerRadius;
        float innerX2 = Mathf.Cos(doorAngle + halfWidth / innerRadius) * innerRadius;
        float innerZ2 = Mathf.Sin(doorAngle + halfWidth / innerRadius) * innerRadius;

        float outerX1 = Mathf.Cos(doorAngle - halfWidth / outerRadius) * outerRadius;
        float outerZ1 = Mathf.Sin(doorAngle - halfWidth / outerRadius) * outerRadius;
        float outerX2 = Mathf.Cos(doorAngle + halfWidth / outerRadius) * outerRadius;
        float outerZ2 = Mathf.Sin(doorAngle + halfWidth / outerRadius) * outerRadius;

        // Door bottom vertices
        vertices.Add(new Vector3(innerX1, 0, innerZ1));
        vertices.Add(new Vector3(innerX2, 0, innerZ2));
        vertices.Add(new Vector3(outerX1, 0, outerZ1));
        vertices.Add(new Vector3(outerX2, 0, outerZ2));

        // Door top vertices
        vertices.Add(new Vector3(innerX1, doorHeight, innerZ1));
        vertices.Add(new Vector3(innerX2, doorHeight, innerZ2));
        vertices.Add(new Vector3(outerX1, doorHeight, outerZ1));
        vertices.Add(new Vector3(outerX2, doorHeight, outerZ2));

        // Side segments around the door
        // Left side
        vertices.Add(new Vector3(innerX1, 0, innerZ1));
        vertices.Add(new Vector3(innerX1, doorHeight, innerZ1));
        vertices.Add(new Vector3(outerX1, 0, outerZ1));
        vertices.Add(new Vector3(outerX1, doorHeight, outerZ1));

        // Right side
        vertices.Add(new Vector3(innerX2, 0, innerZ2));
        vertices.Add(new Vector3(innerX2, doorHeight, innerZ2));
        vertices.Add(new Vector3(outerX2, 0, outerZ2));
        vertices.Add(new Vector3(outerX2, doorHeight, outerZ2));

        // Top segment
        vertices.Add(new Vector3(innerX1, doorHeight, innerZ1));
        vertices.Add(new Vector3(innerX2, doorHeight, innerZ2));
        vertices.Add(new Vector3(outerX1, doorHeight, outerZ1));
        vertices.Add(new Vector3(outerX2, doorHeight, outerZ2));

        // Bottom segment
        vertices.Add(new Vector3(innerX1, 0, innerZ1));
        vertices.Add(new Vector3(innerX2, 0, innerZ2));
        vertices.Add(new Vector3(outerX1, 0, outerZ1));
        vertices.Add(new Vector3(outerX2, 0, outerZ2));

        // Add triangles for door frame
        triangles.AddRange(new int[]
        {
            // Left side
            startVertexIndex, startVertexIndex + 1, startVertexIndex + 2,
            startVertexIndex + 1, startVertexIndex + 3, startVertexIndex + 2,

            // Right side
            startVertexIndex + 4, startVertexIndex + 5, startVertexIndex + 6,
            startVertexIndex + 5, startVertexIndex + 7, startVertexIndex + 6,

            // Top segment
            startVertexIndex + 8, startVertexIndex + 9, startVertexIndex + 10,
            startVertexIndex + 9, startVertexIndex + 11, startVertexIndex + 10,

            // Bottom segment
            startVertexIndex + 12, startVertexIndex + 13, startVertexIndex + 14,
            startVertexIndex + 13, startVertexIndex + 15, startVertexIndex + 14
        });
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(HollowCylinderGenerator))]
    public class HollowCylinderGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            HollowCylinderGenerator script = (HollowCylinderGenerator)target;
            if (GUILayout.Button("Generate Cylinder"))
            {
                script.GenerateHollowCylinder();
            }
        }
    }
#endif
}
