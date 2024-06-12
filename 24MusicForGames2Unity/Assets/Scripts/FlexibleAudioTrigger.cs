using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
#if USE_STEAMVR
using Valve.VR;
#endif

public class FlexibleAudioTrigger : MonoBehaviour
{
    // Assign the FMOD event in the Unity inspector
    public EventReference fmodEvent;

    private FMOD.Studio.EventInstance eventInstance;

    #if USE_STEAMVR
    // SteamVR Input (optional)
    public SteamVR_Input_Sources handType; // The hand to check for input
    public SteamVR_Action_Boolean triggerAction; // The trigger action
    public SteamVR_Behaviour_Pose controllerPose; // The pose behavior for the controller
    #endif

    void Start()
    {
        // Create the FMOD event instance
        eventInstance = RuntimeManager.CreateInstance(fmodEvent);
        Debug.Log("FMOD Event Instance Created");

        #if USE_STEAMVR
        // Optional VR components check
        if (triggerAction == null)
        {
            Debug.LogWarning("Trigger action is not assigned. VR interactions will be disabled.");
        }

        if (controllerPose == null)
        {
            Debug.LogWarning("Controller pose is not assigned. VR interactions will be disabled.");
        }
        #endif
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        #if USE_STEAMVR
        // Check for VR controller trigger only if VR components are assigned
        if (triggerAction != null && controllerPose != null && triggerAction.GetStateDown(handType))
        {
            HandleVRClick();
        }
        #endif
    }

    // Method to handle mouse click
    private void HandleMouseClick()
    {
        // Get the mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object hit by the raycast is this object
            if (hit.transform == transform)
            {
                // Trigger the FMOD event
                TriggerAudio();
            }
        }
    }

    #if USE_STEAMVR
    // Method to handle VR controller click
    private void HandleVRClick()
    {
        // Get the position and rotation of the VR controller
        Ray ray = new Ray(controllerPose.transform.position, controllerPose.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object hit by the raycast is this object
            if (hit.transform == transform)
            {
                // Trigger the FMOD event
                TriggerAudio();
            }
        }
    }
    #endif

    // Method to trigger the audio
    private void TriggerAudio()
    {
        eventInstance.start();
        Debug.Log("FMOD Event Triggered");
    }

    void OnDestroy()
    {
        // Release the event instance when the object is destroyed
        eventInstance.release();
    }
}