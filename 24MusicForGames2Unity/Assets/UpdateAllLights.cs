using UnityEngine;

public class UpdateAllLights : MonoBehaviour
{
    public Color newColor = Color.yellow;

    void Start()
    {
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            light.color = newColor;
        }
    }
}