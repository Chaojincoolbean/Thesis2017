using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HomingMethod
{
    homeWhenFalling
}

public class homing : MonoBehaviour {
    public HomingMethod homingMethod;

    bool targetFound = false;
    Rigidbody rb;
    GameObject player;
    Vector3 holyBladeVelocity;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]");

    }
	
	// Update is called once per frame
	void Update () {
        
        switch (homingMethod)
        {
            case HomingMethod.homeWhenFalling:
                {
                    if (rb.velocity.magnitude < 3 && targetFound == false)
                    {
                        holyBladeVelocity = ((player.transform.position - transform.position).normalized * 80);
                        if (player != null)
                        {
                            //transform.LookAt(player.transform);
                            rb.useGravity = false;
                            rb.velocity = holyBladeVelocity;
                            targetFound = true;
                            //print(gameObject.name+" target");
                        }
                        else
                        {
                            player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]");
                            rb.useGravity = false;
                            rb.velocity = holyBladeVelocity;
                            targetFound = true;
                            //print(gameObject.name);
                        }
                    }
                    else
                    {
                        if (targetFound == true && rb.velocity != holyBladeVelocity)
                        {
                            //rb.velocity = holyBladeVelocity;
                        }
                        return;
                    }
                }
                break;
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (targetFound == true)
        {
            //print(gameObject.name + " true " + collision.collider.gameObject.name);
        }
        else
        {
            //print(gameObject.name+" false "+ collision.collider.gameObject.name);
            //print();
        }
    }
}
