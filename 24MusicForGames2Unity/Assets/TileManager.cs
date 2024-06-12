using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject explosionOriginObject; // GameObject to use as the origin of the explosion
    public float explosionForce = 200f; // Base force of the explosion
    public float explosionRadius = 5f; // Radius of the explosion
    public float destroyDelay = 5f; // Time before tiles are destroyed

    void Start()
    {
        ApplyExplosionForce();
    }

    void ApplyExplosionForce()
    {
        if (explosionOriginObject == null)
        {
            Debug.LogError("Explosion origin object is not set.");
            return;
        }

        Vector3 explosionOrigin = explosionOriginObject.transform.position;

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Rigidbody rb = tile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate direction from explosion origin to the tile
                Vector3 direction = (tile.transform.position - explosionOrigin).normalized;
                float randomForce = explosionForce * Random.Range(0.8f, 1.2f);

                rb.AddForce(direction * randomForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * randomForce); // Add some torque for rotational effect

                // Schedule destruction of the tile
                Destroy(tile, destroyDelay);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (explosionOriginObject != null)
        {
            // Draw the explosion radius in the editor for visualization
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(explosionOriginObject.transform.position, explosionRadius);
        }
    }
}
