using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class PortalManager : MonoBehaviour
{
    [System.Serializable]
    public class DoorSettings
    {
        public GameObject door;
        public string sceneName;
        public EventReference sfx; // FMOD Event Reference for the door's sound effect
    }

    public DoorSettings[] doors;

    void Start()
    {
        foreach (var door in doors)
        {
            if (door.door != null)
            {
                Collider collider = door.door.GetComponent<Collider>();
                if (collider != null)
                {
                    // Ensure the collider is set as a trigger
                    collider.isTrigger = true;
                    Debug.Log("Collider set as trigger for door: " + door.door.name);
                }
                else
                {
                    Debug.LogWarning("Door " + door.door.name + " does not have a Collider component.");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);

        // Check if the collider belongs to the player by comparing tags
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with: " + other.gameObject.name);

            foreach (var door in doors)
            {
                if (door.door != null && other.gameObject == door.door)
                {
                    Debug.Log("Player collided with door: " + door.door.name);
                    StartCoroutine(ChangeScene(door));
                    break;
                }
            }
        }
    }

    IEnumerator ChangeScene(DoorSettings door)
    {
        Debug.Log("Starting scene change process for door: " + door.door.name);

        // Create an instance of the FMOD event
        FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(door.sfx);

        // Check if the event instance is valid
        if (eventInstance.isValid())
        {
            Debug.Log("FMOD Event Instance created successfully.");

            // Start the event and attach it to the door's position
            eventInstance.start();
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(door.door.transform.position));
            Debug.Log("Played sound effect for door: " + door.door.name);

            // Get the length of the event
            int length;
            eventInstance.getDescription(out var eventDescription);
            eventDescription.getLength(out length);
            Debug.Log("Event length: " + length + "ms");

            // Wait for the length of the event (converted from milliseconds to seconds)
            yield return new WaitForSeconds(length / 1000f);

            // Release the event instance
            eventInstance.release();
            Debug.Log("Released event instance for door: " + door.door.name);

            // Load the assigned scene
            Debug.Log("Loading scene: " + door.sceneName);
            SceneManager.LoadScene(door.sceneName);
        }
        else
        {
            Debug.LogError("Failed to create FMOD Event Instance for door: " + door.door.name);
        }
    }
}