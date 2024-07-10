using UnityEngine;
using UnityEngine.UI;

public class UpdateMARCountDisplay : MonoBehaviour
{
    public int requiredMARCount = 5; // Set the required number of enabled MAR elements
    public int currentMARCount = 0;
    public Text marCountText; // Reference to the UI Text component

    void Start()
    {
        // Initial check in case some MAR elements are already enabled at the start
        UpdateMARCount();
        UpdateMARCountText();
    }

    void Update()
    {
        // Continuously check for enabled MAR elements
        UpdateMARCount();
        UpdateMARCountText();
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

    void UpdateMARCountText()
    {
        // Update the UI Text component
        if (marCountText != null)
        {
            marCountText.text = "Current Count: " + currentMARCount + "/10";
        }
        else
        {
            Debug.LogWarning("MAR Count Text component is not assigned.");
        }
    }
}