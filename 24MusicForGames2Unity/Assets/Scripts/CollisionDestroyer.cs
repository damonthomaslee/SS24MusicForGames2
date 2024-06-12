using UnityEngine;

public class CollisionDestroyer : MonoBehaviour
{
    // Public variable to set the tag of objects to destroy
    public string targetTag = "Tile";

    // This function is called when the collider attached to this object detects a trigger event
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision detected with: {other.gameObject.name}, Tag: {other.gameObject.tag}, Layer: {LayerMask.LayerToName(other.gameObject.layer)}");

        // Check if the collided object has the specified tag
        if (other.gameObject.CompareTag(targetTag))
        {
            Debug.Log($"Destroying object: {other.gameObject.name}");
            Destroy(other.gameObject);
        }
    }
}