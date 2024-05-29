using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    public bool useSceneIndex = true;
    public int sceneIndexToLoad;
    public string sceneNameToLoad;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (useSceneIndex)
            {
                SceneChanger.ChangeScene(sceneIndexToLoad);
            }
            else
            {
                SceneChanger.ChangeScene(sceneNameToLoad);
            }
        }
    }
}