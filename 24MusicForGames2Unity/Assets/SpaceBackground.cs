using UnityEngine;

public class SpaceBackground : MonoBehaviour
{
    public Material spaceMaterial; // Assign in Inspector

    void Start()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(500, 500, 500); // Adjust size as needed
        sphere.GetComponent<Renderer>().material = spaceMaterial;
        
        // Invert normals
        sphere.AddComponent<InvertNormals>();
    }
}
