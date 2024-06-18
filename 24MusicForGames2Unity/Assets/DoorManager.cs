using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [System.Serializable]
    public class DoorLight
    {
        public GameObject door;         // The door game object
        public Color lightColor;        // The color of the area light
    }

    public DoorLight[] doors;          // Array of doors with their light settings
    public GameObject areaLightPrefab; // The area light prefab
    public float lightOffsetY = 2.5f;  // The offset to place the light above the door

    void Start()
    {
        foreach (var doorLight in doors)
        {
            if (doorLight.door != null && areaLightPrefab != null)
            {
                // Calculate the world position for the area light
                Vector3 lightPosition = doorLight.door.transform.position + new Vector3(0, lightOffsetY, 0);

                // Instantiate the area light prefab at the calculated position
                GameObject lightInstance = Instantiate(areaLightPrefab, lightPosition, Quaternion.Euler(90, 0, 0));

                // Parent the light to the door
                lightInstance.transform.SetParent(doorLight.door.transform);

                // Configure the light's color
                Light areaLight = lightInstance.GetComponent<Light>();
                if (areaLight != null && areaLight.type == LightType.Area)
                {
                    areaLight.color = doorLight.lightColor;
                }
            }
        }
    }
}