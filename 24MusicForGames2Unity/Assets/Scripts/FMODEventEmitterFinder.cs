using UnityEngine;
using FMODUnity;

public class FMODEventEmitterFinder : MonoBehaviour
{
    void Start()
    {
        FindAllFMODEmitters();
    }

    void FindAllFMODEmitters()
    {
        // Find all objects in the scene with an FMODUnity.StudioEventEmitter component
        StudioEventEmitter[] emitters = FindObjectsOfType<StudioEventEmitter>();

        // Loop through the array and log the names of the objects
        foreach (StudioEventEmitter emitter in emitters)
        {
            Debug.Log("Found FMOD Event Emitter on GameObject: " + emitter.gameObject.name);
        }
    }
}