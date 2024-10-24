using System.Collections.Generic;
using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance;

    private Dictionary<string, string> sceneDoorMappings = new Dictionary<string, string>();

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SceneTracker exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLastDoorForScene(string sceneName, string doorName)
    {
        if (sceneDoorMappings.ContainsKey(sceneName))
        {
            sceneDoorMappings[sceneName] = doorName;
        }
        else
        {
            sceneDoorMappings.Add(sceneName, doorName);
        }

        // Print all scenes visited for debugging
        PrintAllScenesVisited();
    }

    public string GetLastDoorForScene(string sceneName)
    {
        if (sceneDoorMappings.TryGetValue(sceneName, out string doorName))
        {
            return doorName;
        }
        return null;
    }

    private void PrintAllScenesVisited()
    {
        Debug.Log("Scenes Visited:");
        foreach (KeyValuePair<string, string> entry in sceneDoorMappings)
        {
            Debug.Log($"Scene: {entry.Key}, Last Door: {entry.Value}");
        }
    }
}