using UnityEngine;

public class DestroyAfterMARCount : MonoBehaviour
{
public int requiredMARCount = 5; // Set the required number of enabled MAR elements
private int currentMARCount = 0;

void Start()
{
// Initial check in case some MAR elements are already enabled at the start
UpdateMARCount();
}

void Update()
{
// Continuously check for enabled MAR elements
UpdateMARCount();

// Check if the required number of MAR elements are enabled
if (currentMARCount >= requiredMARCount)
{
Destroy(this.gameObject);
}
}

void UpdateMARCount()
{
// Find all objects with the "MAR" tag
GameObject[] marElements = GameObject.FindGameObjectsWithTag("MAR");

// Reset current count
currentMARCount = 0;

// Count how many MAR elements are enabled
foreach (GameObject marElement in marElements)
{
if (marElement.activeInHierarchy)
{
currentMARCount++;
}
}
}
}