using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AISheriff : MonoBehaviour
{

    public GameObject[] helpers;
    public Vector3 target;
    public float followSpeed;
    public float baseFollowSpeed = 6f;
    public float idleSpeed = 2f;
    public float triggerRadius = 20f;

    public bool followingTarget = false;

    public float moveInterval;
    public float minMoveInterval = 0.1f;
    public float maxMoveInterval = 1f;

    public float nextSpawn;
    public GameObject attackerPrefab;
    public int spawnsLeft = 20;

    private float lastTrigger = 0f;
    private Rigidbody rb;
    private EventInstance targetingEvent;

    void Start()
    {
        moveInterval = 3f;
        rb = GetComponent<Rigidbody>();
        helpers = GameObject.FindGameObjectsWithTag("Helper");
        baseFollowSpeed = Random.Range(7f, 15f);
        idleSpeed = Random.Range(5f, 10f);
        triggerRadius = Random.Range(20f, 40f);
        nextSpawn = Random.Range(15.0f, 25.0f);
    }

    void Update()
    {

        followSpeed = (helpers.Length / 2f + baseFollowSpeed) * (spawnsLeft / 20f);

        if (Time.time - moveInterval >= lastTrigger && !followingTarget)
        {
            target = new Vector3(transform.position.x + Random.Range(-50f, 50f), transform.position.y + Random.Range(-3f, 3f), transform.position.z + Random.Range(-50f, 50f));
            target.y = Mathf.Min(transform.position.y, 20f);
            moveInterval = Random.Range(minMoveInterval, maxMoveInterval);
            lastTrigger = Time.time;
        }

        foreach (GameObject obj in helpers)
        {

            if (Vector3.Distance(transform.position, obj.transform.position) < triggerRadius)
            {
                target = obj.transform.position;
                if (!followingTarget)
                {
                    targetingEvent = RuntimeManager.CreateInstance("event:/Florian/SheriffAction");
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

        if (Time.time > nextSpawn)
            UpdateAI();

    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(transform.position, target, (followingTarget ? followSpeed : idleSpeed) * Time.fixedDeltaTime));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Helper")
        {
            collision.gameObject.tag = "Untagged";
            EventInstance killEvent = RuntimeManager.CreateInstance("event:/Florian/Kill");
            RuntimeManager.AttachInstanceToGameObject(killEvent, gameObject.transform);
            killEvent.start();
            killEvent.release();
            Destroy(collision.gameObject);

            foreach (GameObject sheriff in GameObject.FindGameObjectsWithTag("Sheriff"))
            {
                sheriff.GetComponent<AISheriff>().helpers = GameObject.FindGameObjectsWithTag("Helper");
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

    private void UpdateAI()
    {
        GameObject newAttacker = Instantiate(attackerPrefab);
        newAttacker.transform.position = transform.position;        

        foreach (GameObject helper in GameObject.FindGameObjectsWithTag("Helper"))
        {
            helper.GetComponent<AIHelper>().attackers = GameObject.FindGameObjectsWithTag("Attacker");
        }

        nextSpawn = Time.time + Random.Range(5.0f, 15.0f);
        spawnsLeft--;

        //if (spawnsLeft == 0)
        //    Destroy(gameObject);

    }

}
