using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using VRTK;

public class fuckingFuckTellMeWhatTheFuckingFootIsTouching : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.collider.name);
        float impact = collision.relativeVelocity.magnitude;
        print(impact);
        //If whatever stuff collide with this fist has a rigidbody
        if (collision.collider.GetComponent<Rigidbody>() != null)
        {
            //If this rigidbody is a puppet muscle
            if (collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>() != null)
            {
                //Damage the mannequin enemy depend on where the player hit and print its remaining health
                if (collision.collider.name == "Head")
                {
                    collision.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= impact * 2f;
                }
                else
                {
                    collision.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= impact;
                }

                print(collision.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health);

            }
            else
            {
                // Not a muscle (any more)
                var joint = collision.collider.attachedRigidbody.GetComponent<ConfigurableJoint>();
                if (joint != null) Destroy(joint);
            }
        }
    }
}

