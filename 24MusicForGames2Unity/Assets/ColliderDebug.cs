using UnityEngine;

public class ColliderDebug : MonoBehaviour
{
    void Start()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
        {
            Debug.Log("Collider found on " + col.gameObject.name + ", isTrigger: " + col.isTrigger);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name + " on " + gameObject.name);
 
    }
    
}