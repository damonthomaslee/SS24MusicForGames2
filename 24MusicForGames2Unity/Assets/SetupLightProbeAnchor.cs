using UnityEngine;

public class SetupLightProbeAnchor : MonoBehaviour
{
    public Transform anchorTransform;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && anchorTransform != null)
        {
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.CustomProvided;
            renderer.probeAnchor = anchorTransform;
        }
    }
}