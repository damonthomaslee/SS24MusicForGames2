using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleDoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collided with door: {gameObject.name}");
            DoorManagerSwitchRandom doorManager = FindObjectOfType<DoorManagerSwitchRandom>();
            if (doorManager != null)
            {
                doorManager.HandleDoorTrigger(gameObject);
                SceneTracker.Instance.SetLastDoorForScene(SceneManager.GetActiveScene().name, gameObject.name);
            }
            else
            {
                Debug.LogWarning("DoorManagerSwitchRandom not found.");
            }
        }
    }
}