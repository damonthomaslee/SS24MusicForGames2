using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AIHelper : MonoBehaviour
{

    public GameObject[] attackers;
    public Vector3 target;
    public float followSpeed = 6f;
    public float idleSpeed = 2f;
    public float triggerRadius = 20f;

    public bool followingTarget = false;

    public float moveInterval;
    public float minMoveInterval = 2f;
    public float maxMoveInterval = 5f;

    private float lastTrigger = 0f;
    private Rigidbody rb;
    private EventInstance targetingEvent;

    void Start()
    {
        moveInterval = 3f;
        rb = GetComponent<Rigidbody>();
        attackers = GameObject.FindGameObjectsWithTag("Attacker");
        followSpeed = Random.Range(4f, 10f);
        idleSpeed = Random.Range(1.5f, 3f);
        triggerRadius = Random.Range(10f, 30f);
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - moveInterval >= lastTrigger && !followingTarget)
        {
            target = new Vector3(transform.position.x + Random.Range(-5f, 5f), transform.position.y + Random.Range(-3f, 3f), transform.position.z + Random.Range(-5f, 5f));
            target.y = Mathf.Min(transform.position.y, 20f);
            moveInterval = Random.Range(minMoveInterval, maxMoveInterval);
            lastTrigger = Time.time;
        }

        foreach (GameObject obj in attackers) {

            if (Vector3.Distance(transform.position, obj.transform.position) < triggerRadius)
            {
                target = obj.transform.position;
                if (!followingTarget)
                {
                    targetingEvent = RuntimeManager.CreateInstance("event:/Florian/HelperAction");
                    RuntimeManager.AttachInstanceToGameObject(targetingEvent, gameObject.transform);
                    targetingEvent.start();
                    targetingEvent.release();
                    followingTarget = true;
                }
                return;
            }

        }
        
        if (followingTarget)
        {
            followingTarget = false;
            targetingEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(transform.position, target, (followingTarget ? followSpeed : idleSpeed) * Time.fixedDeltaTime));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Attacker")
        {
            collision.gameObject.tag = "Untagged";
            EventInstance killEvent = RuntimeManager.CreateInstance("event:/Florian/Kill");
            RuntimeManager.AttachInstanceToGameObject(killEvent, gameObject.transform);
            killEvent.start();
            killEvent.release();
            Destroy(collision.gameObject);

            foreach (GameObject helper in GameObject.FindGameObjectsWithTag("Helper")) {
                helper.GetComponent<AIHelper>().attackers = GameObject.FindGameObjectsWithTag("Attacker");
            }

            StartCoroutine(SpeedCooldown());
        }
    }

    IEnumerator SpeedCooldown()
    {
        idleSpeed *= 2;
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        idleSpeed /= 2;
    }

}
