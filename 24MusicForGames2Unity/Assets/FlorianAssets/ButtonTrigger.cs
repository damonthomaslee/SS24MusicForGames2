using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject helperPrefab;
    public float lastTrigger = 0f;
    public bool canTrigger = true;
    public Material mat;

    void Start()
    {
        mat = GetComponentInParent<MeshRenderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (canTrigger)
        {
            if (!other.CompareTag("Player"))
                return;

            FMOD.Studio.EventInstance triggerEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Florian/Button");
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(triggerEvent, gameObject.transform);
            triggerEvent.start();
            triggerEvent.release();

            Destroy(GameObject.Find("NoiseMaker"));

            for (int i = -100; i <= 100; i += 20)
            { 
                for (int j = -100; j <= 100; j += 20)
                {
                    GameObject newHelper = Instantiate(helperPrefab);
                    newHelper.transform.position = new Vector3(i + Random.Range(-10f, 10f), 20 + Random.Range(0f, 5f), j + Random.Range(-10f, 10f));
                }
            }

            foreach (GameObject sheriff in GameObject.FindGameObjectsWithTag("Sheriff"))
            {
                sheriff.GetComponent<AISheriff>().helpers = GameObject.FindGameObjectsWithTag("Helper");
            }

            StartCoroutine(Cooldown());
        }            
    }

    IEnumerator Cooldown()
    {
        canTrigger = false;
        mat.color = new Color(0, 0, 0, 255);
        yield return new WaitForSeconds(Random.Range(40f, 80f));
        mat.color = new Color(255, 215, 0, 255);
        canTrigger = true;
    }
}
