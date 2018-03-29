using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwingKnifeBlade : MonoBehaviour {
    public float collisionMagnitude;
    public float parentDelay = 0.1f;
    public float damage = 15f;
    public AudioClip[] clips;
    public int bounceCount;

    private int i;
    private AudioSource source;
    [SerializeField] private GameObject playerCamera;
    // Use this for initialization
    void Start () {
        playerCamera = GameObject.Find("Camera (eye)");
        source = gameObject.GetComponent<AudioSource>();

        //Play the throwing sound effect
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        i += 1;

        if (i > bounceCount)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GameObject objectHit = collision.collider.gameObject;
            if (collision.relativeVelocity.magnitude > collisionMagnitude)
            {
                if (gameObject.GetComponent<FixedJoint>() == null)
                {
                    FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = objectHit.GetComponent<Rigidbody>();
                    print(gameObject.name + " Is a child of " + objectHit.name);
                }

                //Throwing knife attaching by parenting, not used now
                /*Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                Destroy(rb);
                transform.parent = hitted.transform;*/


            }
        }
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
}
