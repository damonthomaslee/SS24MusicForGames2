using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeeSimpleDistance : MonoBehaviour
{
    public Transform other;
    public string bodypart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (other)
        {
            float dist = Vector3.Distance(other.position, transform.position);
            //Debug.Log("Distance to other: " + dist);
            GetComponent<FMODUnity.StudioEventEmitter>().SetParameter(bodypart, dist, false);
         
        }

    }
    
}
