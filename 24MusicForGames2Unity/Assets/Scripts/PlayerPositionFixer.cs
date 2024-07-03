using UnityEngine;

public class PlayerPositionFixer : MonoBehaviour
{
    public GameObject player;
    public Vector3 startPosition = new Vector3(0, 1, 0);

    private Vector3 lastPosition;

    void Start()
    {
        if (player != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller == null)
            {
                controller = player.AddComponent<CharacterController>();
            }

            player.transform.position = startPosition;
            lastPosition = player.transform.position;
        }
    }

    void Update()
    {
        // Debug log to monitor position changes
        if (player.transform.position != lastPosition)
        {
            Debug.LogWarning($"Player position changed from {lastPosition} to {player.transform.position}");
            lastPosition = player.transform.position;

            // Reset position if it changes unexpectedly
            if (player.transform.position.y > 10 || player.transform.position.y < -1)
            {
                Debug.LogWarning("Player position reset due to unexpected change.");
                player.transform.position = startPosition;
                lastPosition = player.transform.position;
            }
        }
    }
}