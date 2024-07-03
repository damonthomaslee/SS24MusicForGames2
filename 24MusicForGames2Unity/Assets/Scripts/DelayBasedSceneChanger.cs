using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DelayBasedSceneChanger : MonoBehaviour
{
    public float delayInSeconds = 60f; // Public variable for delay, default is 60 seconds
public int sceneNumber = 0;
    private void Start()
    {
        Invoke("ChangeScene", delayInSeconds); // Call ChangeScene after the specified delay
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneNumber); // Change to the next scene in build order
    }
}