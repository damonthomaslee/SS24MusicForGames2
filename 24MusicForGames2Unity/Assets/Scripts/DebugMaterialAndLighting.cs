using UnityEngine;

public class DebugMaterialAndLighting : MonoBehaviour
{
    public GameObject floorObject;
    public Color expectedLightColor = Color.yellow;

    void Start()
    {
        // Check the material properties
        if (floorObject != null)
        {
            Renderer renderer = floorObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Debug.Log("Floor Material: " + renderer.material.name);
                Debug.Log("Emission Color: " + renderer.material.GetColor("_EmissionColor"));
            }
        }

        // Check all light colors
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            Debug.Log("Light: " + light.name + " Color: " + light.color);
            if (light.color != expectedLightColor)
            {
                Debug.LogWarning("Light color mismatch: " + light.name);
            }
        }
    }
}