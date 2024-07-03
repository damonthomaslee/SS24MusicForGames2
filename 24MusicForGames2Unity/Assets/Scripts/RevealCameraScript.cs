using UnityEngine;
using Cinemachine;

public class RevealCameraScript : MonoBehaviour
{
    public float delay = 2f; // Time to wait before changing priority

    private CinemachineVirtualCamera virtualCamera;

    private void OnEnable()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            Debug.Log("RevealCameraScript: Camera enabled, starting delay timer.");
            Invoke("SwitchPriority", delay);
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera component not found on the GameObject.");
        }
    }

    private void SwitchPriority()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Priority = 9;
            Debug.Log("RevealCameraScript: Reveal camera priority set to 9.");
        }
    }
}
