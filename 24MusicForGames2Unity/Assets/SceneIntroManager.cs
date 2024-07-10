using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneIntroManager : MonoBehaviour
{
    public Canvas introCanvas; // Reference to the Canvas
    public Text introText; // Reference to the Text component
    private static bool hasSeenIntro = false; // Static variable to track if the intro has been seen
    public float showDelay = 2f; // Delay before showing the intro
    public float hideDelay = 10f; // Delay before hiding the intro

    void Start()
    {
        if (!hasSeenIntro)
        {
            StartCoroutine(ShowAndHideIntro());
            hasSeenIntro = true; // Mark the intro as seen
        }
    }

    IEnumerator ShowAndHideIntro()
    {
        yield return new WaitForSeconds(showDelay); // Wait for the show delay
        ShowIntro();
        yield return new WaitForSeconds(hideDelay); // Wait for the hide delay
        HideIntro();
    }

    void ShowIntro()
    {
        introCanvas.gameObject.SetActive(true);
        introText.text = "Choose a door, and walk through it"; // Set the message
    }

    void HideIntro()
    {
        introCanvas.gameObject.SetActive(false);
    }
}