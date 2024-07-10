using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        // Find all instances of MusicManager
        MusicManager[] managers = FindObjectsOfType<MusicManager>();
        foreach (var manager in managers)
        {
            if (manager != this)
            {
                // Destroy all other instances
                Destroy(manager.gameObject);
            }
        }

        // Set this instance as the singleton instance
        instance = this;
        DontDestroyOnLoad(gameObject);
       // Debug.Log("MusicManager instance created.");
    }

    void OnDestroy()
    {
        // Ensure instance is null if this instance is destroyed
        if (instance == this)
        {
            instance = null;
        }
    }

    public static void DestroyMusicManager()
    {
        if (instance != null)
        {
            Debug.Log("Destroying MusicManager instance.");
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}