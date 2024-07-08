using UnityEngine;

public class AssignMaterials : MonoBehaviour
{
    public GameObject marimbaObject;
    public Material[] materials;

    void Start()
    {
        Renderer[] renderers = marimbaObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = materials[i % materials.Length];
        }
    }
}