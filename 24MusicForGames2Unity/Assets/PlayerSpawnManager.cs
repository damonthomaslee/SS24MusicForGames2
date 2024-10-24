using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform defaultSpawnPoint;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        string previousScene = SceneManager.GetActiveScene().name;
        string lastDoorName = SceneTracker.Instance.GetLastDoorForScene(previousScene);

        if (!string.IsNullOrEmpty(lastDoorName))
        {
            GameObject lastDoor = GameObject.Find(lastDoorName);
            if (lastDoor != null)
            {
                Transform spawnPoint = lastDoor.transform;
                // Adjust spawn point to be in front of the door
                Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * 2f; // Adjust the multiplier as needed
                Quaternion spawnRotation = Quaternion.LookRotation(-spawnPoint.forward); // Face the door
                transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            }
            else
            {
                transform.position = defaultSpawnPoint.position;
                transform.rotation = defaultSpawnPoint.rotation;
            }
        }
        else
        {
            transform.position = defaultSpawnPoint.position;
            transform.rotation = defaultSpawnPoint.rotation;
        }
    }
}