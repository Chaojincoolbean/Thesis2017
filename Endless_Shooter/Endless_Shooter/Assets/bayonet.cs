using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class bayonet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        print("Bayonet has stabbed " + other.gameObject.name + other.gameObject.tag);
        if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.tag != "Weapon")
        {
            transform.parent.gameObject.AddComponent<FixedJoint>();
            gameObject.GetComponent<FixedJoint>().connectedBody = other.gameObject.GetComponent<Rigidbody>();
            gameObject.GetComponent<FixedJoint>().breakForce = 1500;
            gameObject.GetComponent<FixedJoint>().breakTorque = 1500;
        }
    }
}
