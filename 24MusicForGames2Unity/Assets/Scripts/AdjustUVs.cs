using UnityEngine;

public class AdjustUVs : MonoBehaviour
{
    public Vector2 tiling = new Vector2(1, 1);
    public Vector2 offset = new Vector2(0, 0);

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uvs = mesh.uv;

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = Vector2.Scale(uvs[i], tiling) + offset;
        }

        mesh.uv = uvs;
    }
}