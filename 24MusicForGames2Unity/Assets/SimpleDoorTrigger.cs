using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class SimpleDoorTrigger : MonoBehaviour
{
    public string sceneName; // Name of the scene to load
    public EventReference sfx; // FMOD Event Reference for the door's sound effect
    public FadeUIController fadeController; // Reference to the FadeUIController
    public float fadeDuration = 1f; // Duration of the fade effect

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);

        // Check if the collider belongs to the player by comparing tags
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with door: " + gameObject.name);
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        Debug.Log("Starting scene change process for door: " + gameObject.name);

        // Start the fade to black
        if (fadeController != null)
        {
            fadeController.FadeToBlack(fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }

        // Create an instance of the FMOD event
        FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(sfx);

        // Check if the event instance is valid
        if (eventInstance.isValid())
        {
            Debug.Log("FMOD Event Instance created successfully.");

            // Start the event and attach it to the door's position
            eventInstance.start();
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            Debug.Log("Played sound effect for door: " + gameObject.name);

            // Get the length of the event
            int length;
            eventInstance.getDescription(out var eventDescription);
            eventDescription.getLength(out length);
            Debug.Log("Event length: " + length + "ms");

            // Wait for the length of the event (converted from milliseconds to seconds)
            yield return new WaitForSeconds(length / 1000f);

            // Release the event instance
            eventInstance.release();
            Debug.Log("Released event instance for door: " + gameObject.name);

            // Load the assigned scene
            Debug.Log("Loading scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Failed to create FMOD Event Instance for door: " + gameObject.name);
        }
    }
}