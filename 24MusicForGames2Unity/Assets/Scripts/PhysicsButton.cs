using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    public Rigidbody buttonTopRigid;
    public Transform buttonTop;
    public Transform buttonLowerLimit;
    public Transform buttonUpperLimit;
    public float threshHold;
    public float force = 10;
    private float upperLowerDiff;
    public bool isPressed;
    private bool prevPressedState;
    public AudioClip pressedSound;
    public AudioClip releasedSound;
    private AudioSource source;
    public Collider[] CollidersToIgnore;
    public UnityEvent onPressed;
    public UnityEvent onReleased;
    public GameObject[] planets;

    void Start()
    {
        source = GetComponent<AudioSource>();
        Collider localCollider = GetComponent<Collider>();
        if (localCollider != null)
        {
            Physics.IgnoreCollision(localCollider, buttonTop.GetComponentInChildren<Collider>());

            foreach (Collider singleCollider in CollidersToIgnore)
            {
                Physics.IgnoreCollision(localCollider, singleCollider);
            }
        }

        if (transform.eulerAngles != Vector3.zero)
        {
            Vector3 savedAngle = transform.eulerAngles;
            transform.eulerAngles = Vector3.zero;
            upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
            transform.eulerAngles = savedAngle;
        }
        else
        {
            upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
        }
    }

    void Update()
    {
        buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
        buttonTop.transform.localEulerAngles = new Vector3(0, 0, 0);
        if (buttonTop.localPosition.y >= 0)
        {
            buttonTop.transform.position = new Vector3(buttonUpperLimit.position.x, buttonUpperLimit.position.y, buttonUpperLimit.position.z);
        }
        else
        {
            buttonTopRigid.AddForce(buttonTop.transform.up * force * Time.deltaTime);
        }

        if (buttonTop.localPosition.y <= buttonLowerLimit.localPosition.y)
        {
            buttonTop.transform.position = new Vector3(buttonLowerLimit.position.x, buttonLowerLimit.position.y, buttonLowerLimit.position.z);
        }

        if (Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < upperLowerDiff * threshHold)
        {
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }

        if (isPressed && prevPressedState != isPressed)
        {
            Pressed();
        }
        if (!isPressed && prevPressedState != isPressed)
        {
            Released();
        }
    }

    void Pressed()
    {
        prevPressedState = isPressed;
        if (pressedSound != null)
        {
            source.pitch = 1;
            source.PlayOneShot(pressedSound);
            Debug.Log("Ton ab");
        }
        onPressed.Invoke();
    }

    void Released()
    {
        prevPressedState = isPressed;
        if (releasedSound != null)
        {
            source.pitch = Random.Range(1.1f, 1.2f);
            source.PlayOneShot(releasedSound);
        }
        FMOD.Studio.EventInstance bgm = FMODUnity.RuntimeManager.CreateInstance("event:/Mathis_WeltRAUM/BackgroundMusic");
        bgm.start();
        bgm.release();
        onReleased.Invoke();
        StartDestructionProcess();

    }

    public void StartDestructionProcess()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
          Debug.Log("Destroying tile: " + tile.name + " with tag: " + tile.tag);
          StartCoroutine(DestroyTileAfterRandomDelay(tile, Random.Range(3f, 15f)));
        }


        foreach (GameObject planet in planets)
        {
          planet.SetActive(true);
        }
    }

    private IEnumerator DestroyTileAfterRandomDelay(GameObject tile, float delayInSeconds)
    {
        Debug.Log("Tile " + tile.name + " will be destroyed in " + delayInSeconds + " seconds. Current tag: " + gameObject.tag);
        yield return new WaitForSeconds(delayInSeconds);
        Destroy(tile); // Anpassung, um spezifische Kachel zu zerstören
    }
}
