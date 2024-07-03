using UnityEngine;

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
            }
            else
            {
                Debug.LogWarning("DoorManagerSwitchRandom not found.");
            }
        }
    }
}