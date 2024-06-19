using UnityEngine;

public class ApplyMaterial : MonoBehaviour
{
    public Material material;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }
}