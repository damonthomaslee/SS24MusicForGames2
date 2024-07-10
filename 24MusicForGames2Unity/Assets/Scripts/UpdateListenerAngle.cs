using UnityEngine;

public class UpdateListenerAngle : MonoBehaviour
{
    // Use the EventReference struct for the FMOD event
    public FMODUnity.EventReference eventReference;
    private FMOD.Studio.EventInstance eventInstance;
    public Transform listener; // Reference to the listener (e.g., the player)
    public float updateInterval = 0.1f; // Time interval for updates
    private float nextUpdateTime = 0f;
    public float maxDistance = 50f; // Maximum distance for updates

    void Start()
    {
        // Create an instance of the FMOD event using the EventReference struct
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
        eventInstance.start();
    }

    void Update()
    {
        // Update the listener angle at defined intervals
        if (Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateInterval;

            float distance = Vector3.Distance(listener.position, transform.position);
            if (distance <= maxDistance)
            {
                // Set the 3D attributes of the event instance
                FMOD.ATTRIBUTES_3D attributes = FMODUnity.RuntimeUtils.To3DAttributes(transform);
                eventInstance.set3DAttributes(attributes);
            }
        }
    }

    void OnDestroy()
    {
        // Stop and release the event instance when the object is destroyed
        eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        eventInstance.release();
    }
}