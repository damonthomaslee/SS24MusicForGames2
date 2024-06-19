using UnityEngine;

public class VerifyLightColors : MonoBehaviour
{
    public Color correctColor = Color.yellow;

    void Start()
    {
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            light.color = correctColor;
        }
    }
}