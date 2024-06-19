using UnityEngine;

public class RoofGenerator : MonoBehaviour
{
    public float radius = 10f;                 // Radius of the main structure
    public float height = 10f;                 // Height at which to place the roof
    public GameObject roofPrefab;              // Prefab of the roof (should be a cylinder)
    public Material emissiveMaterial;          // Emissive material for the roof

    void Start()
    {
        if (roofPrefab != null)
        {
            // Instantiate the roof prefab at the specified height
            GameObject roof = Instantiate(roofPrefab, new Vector3(0, height, 0), Quaternion.identity);

            // Adjust the scale to cover the structure
            roof.transform.localScale = new Vector3(radius * 2, 0.1f, radius * 2);

            // Optionally, set the roof as a child of the main structure to ensure it moves with it
            roof.transform.SetParent(transform, false);

            // Apply the emissive material to the roof
            Renderer roofRenderer = roof.GetComponent<Renderer>();
            if (roofRenderer != null && emissiveMaterial != null)
            {
                roofRenderer.material = emissiveMaterial;
                Debug.Log("Material assigned: " + roofRenderer.material.name);
            }
            else
            {
                Debug.LogError("Roof prefab does not have a Renderer component or emissiveMaterial is not assigned.");
            }
        }
        else
        {
            Debug.LogError("Roof prefab is not assigned.");
        }
    }
}