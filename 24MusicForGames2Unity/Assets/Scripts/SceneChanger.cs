using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void ChangeScene(int sceneIndex)
    {
        Debug.Log("Changing scene to index: " + sceneIndex);
        MusicManager.DestroyMusicManager();
        SceneManager.LoadScene(sceneIndex);
    }

    public static void ChangeScene(string sceneName)
    {
        Debug.Log("Changing scene to name: " + sceneName);
        MusicManager.DestroyMusicManager();
        SceneManager.LoadScene(sceneName);
    }
}