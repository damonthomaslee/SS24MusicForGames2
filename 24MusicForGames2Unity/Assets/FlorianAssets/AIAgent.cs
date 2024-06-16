using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AIAgent : MonoBehaviour
{

    public Vector3 target;
    public float followSpeed = 4f;
    public float idleSpeed = 2f;
    public float triggerRadius = 20f;

    public int directionChange = 300;

    public bool followingPlayer = false;

    public float moveInterval;
    public float minMoveInterval = 2f;
    public float maxMoveInterval = 5f;

    private GameObject player;
    private float lastTrigger = 0f;
    private Rigidbody rb;
    private EventInstance targetingEvent;

    // Start is called before the first frame update
    void Start()
    {
        moveInterval = 3f;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        //RuntimeManager.AttachInstanceToGameObject(targetingEvent, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {


        if (Time.time - moveInterval >= lastTrigger && !followingPlayer)
        {
            target = new Vector3(transform.position.x + Random.Range(-5f, 5f), transform.position.y + Random.Range(-3f, 3f), transform.position.z + Random.Range(-5f, 5f));
            target.y = Mathf.Min(transform.position.y, 20f);
            moveInterval = Random.Range(minMoveInterval, maxMoveInterval);
            lastTrigger = Time.time;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < triggerRadius)
        {
            target = player.transform.position;
            if (!followingPlayer)
            {
                targetingEvent = RuntimeManager.CreateInstance("event:/Florian/SpotPlayer");
                RuntimeManager.AttachInstanceToGameObject(targetingEvent, gameObject.transform);
                targetingEvent.start();
                targetingEvent.release();
                followingPlayer = true;
            }
        } else if (followingPlayer)
        {
            followingPlayer = false;
            targetingEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(transform.position, target, (followingPlayer ? followSpeed : idleSpeed) * Time.fixedDeltaTime));
    }

    void OnDestroy()
    {
        targetingEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
