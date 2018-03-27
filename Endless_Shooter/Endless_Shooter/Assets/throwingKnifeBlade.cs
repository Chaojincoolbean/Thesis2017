using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwingKnifeBlade : MonoBehaviour {
    public float collisionMagnitude;
    public float parentDelay = 0.1f;
    public float damage = 15f;
    [SerializeField] private GameObject playerCamera;
    // Use this for initialization
    void Start () {
        playerCamera = GameObject.Find("Camera (eye)");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        print(collision.relativeVelocity.magnitude + "From dagger blade");
        GameObject objectHit = collision.collider.gameObject;
        if (collision.relativeVelocity.magnitude > collisionMagnitude)
        {
            StartCoroutine(ChildMyself(objectHit));
        }
    }

    IEnumerator ChildMyself(GameObject hitted)
    {
        yield return new WaitForSeconds(parentDelay);
        transform.parent.parent = hitted.transform;
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
