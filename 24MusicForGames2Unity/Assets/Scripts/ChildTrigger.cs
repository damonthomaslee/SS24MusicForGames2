using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    public GameObject ParentDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (ParentDoor != null)
        {
            SimpleDoorTrigger doorTrigger = ParentDoor.GetComponent<SimpleDoorTrigger>();
            if (doorTrigger != null)
            {
                //doorTrigger.TriggerDoor(other);
            }
        }
    }
}