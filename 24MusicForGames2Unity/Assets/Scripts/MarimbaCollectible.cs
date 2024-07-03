using UnityEngine;
using Cinemachine;
using System.Collections;

public class MarimbaCollectible : MonoBehaviour
{
    public GameObject nextObject; // The object to enable after collecting this one
    public float panDuration = 2f; // Duration for the camera pan
    public float birdEyeViewDuration = 3f; // Duration to stay in the bird's eye view
    public CinemachineVirtualCamera revealCamera; // The camera to activate when this object is revealed
    public CinemachineVirtualCamera playerFollowCamera; // The main player follow camera
    private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        // Ensure the CinemachineBrain is on the main camera
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        if (cinemachineBrain == null)
        {
            Debug.LogError("CinemachineBrain component not found on the main camera.");
        }

        // Ensure the reveal camera is disabled initially
        if (revealCamera != null)
        {
            revealCamera.gameObject.SetActive(false);
            Debug.Log("Reveal camera disabled at start.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (nextObject != null)
            {
                StartCoroutine(EnableNextObject());
            }
            Destroy(gameObject); // Destroy the collectible
        }
    }

    private IEnumerator EnableNextObject()
    {
        // Enable the next object
        nextObject.SetActive(true);
        Debug.Log("Next object enabled.");

        // Activate the reveal camera
        if (revealCamera != null)
        {
            Debug.Log("Activating reveal camera.");
            revealCamera.gameObject.SetActive(true);
            revealCamera.Priority = 20; // Ensure this camera has higher priority to take control
            Debug.Log("Reveal camera priority set to 20.");
        }

        // Player follow camera stays at priority 9
        Debug.Log("Player follow camera priority remains at 9.");

        // Wait for the bird's eye view duration
        yield return new WaitForSeconds(birdEyeViewDuration);
        Debug.Log("Bird's eye view duration elapsed.");

        // Enable the RevealCameraScript to change the priority back to 9 after delay
        revealCamera.GetComponent<RevealCameraScript>().enabled = true;

        // Wait for the pan duration to complete the transition back
        yield return new WaitForSeconds(panDuration);
        Debug.Log("Pan duration elapsed.");
    }
}
