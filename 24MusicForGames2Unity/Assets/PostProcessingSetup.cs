using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingSetup : MonoBehaviour
{
    public VolumeProfile profile;

    void Start()
    {
        var volume = gameObject.AddComponent<Volume>();
        volume.profile = profile;
        volume.isGlobal = true;

        // Ensure the camera is set to apply post-processing effects
        Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = true;
    }
}