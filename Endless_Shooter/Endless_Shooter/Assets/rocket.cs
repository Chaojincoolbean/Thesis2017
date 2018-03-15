using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour {
    public float propellingForce;
    public float torque;
    public GameObject tail;

    // Use this for initialization
    void Start () {
        GetComponent<ConstantForce>().force = transform.forward * 100;
        GetComponent<ConstantForce>().relativeTorque = transform.right * 5;
    }
	
	// Update is called once per frame
	void Update () {

    }
}
