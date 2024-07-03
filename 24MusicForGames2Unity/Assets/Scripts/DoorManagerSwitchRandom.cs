using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DoorManagerSwitchRandom : MonoBehaviour
{
    [System.Serializable]
    public class DoorAssignment
    {
        public GameObject door;
        public int sceneIndex;
        public EventReference sfx;
    }

    public List<DoorAssignment> doorAssignments; // List of door assignments
    public bool randomize = false; // Flag to randomize door assignments

    private Dictionary<GameObject, int> doorSceneMap = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, EventReference> doorSfxMap = new Dictionary<GameObject, EventReference>();

    private void Start()
    {
        if (randomize)
        {
            ShuffleDoors();
        }
        AssignDoors();
    }

    public void HandleDoorTrigger(GameObject door)
    {
        if (doorSceneMap.ContainsKey(door) && doorSfxMap.ContainsKey(door))
        {
            int sceneIndex = doorSceneMap[door];
            EventReference sfxEvent = doorSfxMap[door];

            Debug.Log($"Triggering door: {door.name}, Scene Index: {sceneIndex}, SFX: {sfxEvent}");

            PlaySFXAndChangeScene(sfxEvent, sceneIndex);
        }
        else
        {
            Debug.LogWarning($"Door trigger or doorSceneMap entry not found for door: {door.name}");
        }
    }

    private void AssignDoors()
    {
        foreach (var assignment in doorAssignments)
        {
            if (assignment.door != null)
            {
                doorSceneMap[assignment.door] = assignment.sceneIndex;
                doorSfxMap[assignment.door] = assignment.sfx;
                Debug.Log($"Assigned Door: {assignment.door.name}, Scene Index: {assignment.sceneIndex}, SFX: {assignment.sfx}");
            }
        }
    }

    private void ShuffleDoors()
    {
        // Store the original assignments for logging purposes
        List<DoorAssignment> originalAssignments = new List<DoorAssignment>(doorAssignments);

        // Shuffle the door assignments
        for (int i = doorAssignments.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = doorAssignments[i];
            doorAssignments[i] = doorAssignments[j];
            doorAssignments[j] = temp;
        }

        // Log the reassignment details
        for (int i = 0; i < doorAssignments.Count; i++)
        {
            var originalAssignment = originalAssignments[i];
            var newAssignment = doorAssignments[i];
            Debug.Log($"Door {originalAssignment.door.name} reassigned from Scene {originalAssignment.sceneIndex} to Scene {newAssignment.sceneIndex}");
        }
    }

    private void PlaySFXAndChangeScene(EventReference sfx, int sceneIndex)
    {
        FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(sfx);
        if (eventInstance.isValid())
        {
            eventInstance.start();
            eventInstance.release();

            Debug.Log($"Playing SFX and immediately changing scene to index: {sceneIndex}");

            // Change the scene immediately
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"FMOD event instance is not valid");
        }
    }
}