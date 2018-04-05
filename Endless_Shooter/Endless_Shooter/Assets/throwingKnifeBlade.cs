using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwingKnifeBlade : MonoBehaviour {
    public float collisionMagnitude;
    public float parentDelay = 0.1f;
    public float damage = 15f;
    public AudioClip[] clips;
    public int bounceCount;
    public bool forceRotation = true;
    public GameObject destroyParticle;

    private int i;
    private AudioSource source;
    private Rigidbody rb;
    private bool isFlying = true;
    [SerializeField] private GameObject playerCamera;
    // Use this for initialization
    void Start () {
        playerCamera = GameObject.Find("Camera (eye)");
        source = gameObject.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        //Play the throwing sound effect
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();

        Invoke("DeathParticle", 15f);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (isFlying == true && forceRotation == true)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        i += 1;
        StartCoroutine(ApplyForceAfterBounce());
        if (i > bounceCount)
        {
            isFlying = false;
            GetComponent<Rigidbody>().useGravity = true;
            GameObject objectHit = collision.collider.gameObject;
            if (collision.relativeVelocity.magnitude > collisionMagnitude)
            {
                if (gameObject.GetComponent<homing>() != null)
                {
                    //gameObject.GetComponent<homing>().enabled = false;
                }
                if (gameObject.GetComponent<FixedJoint>() == null)
                {
                    FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = objectHit.GetComponent<Rigidbody>();
                    //sprint(gameObject.name + " Is a child of " + objectHit.name);
                }

                //Throwing knife attaching by parenting, not used now
                /*Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                Destroy(rb);
                transform.parent = hitted.transform;*/
            }
        }
    }

    IEnumerator ApplyForceAfterBounce()
    {
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        rb.AddForce(rb.velocity * 1.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "[VRTK][AUTOGEN][HeadsetColliderContainer]" || other.name == "[VRTK][AUTOGEN][BodyColliderContainer]")
        {
            print("Player Hit");
            playerCamera.GetComponent<playerHit>().PlayHitSound();
            playerCamera.GetComponent<playerHit>().CameraShake();
            playerCamera.GetComponent<playerHit>().PlayerHealthDecrease(damage);
        }
    }

    void DeathParticle()
    {
        GameObject particle = Instantiate(destroyParticle, gameObject.transform.position, gameObject.transform.rotation);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();

        Destroy(gameObject);
        Destroy(particle, 5f);
    }
}
