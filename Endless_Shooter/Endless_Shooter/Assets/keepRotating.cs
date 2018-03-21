using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepRotating : MonoBehaviour {
    public float YAxisRotatingSpeed = 100f;
    public float XAxisRotatingSpeed = 25f;
    public float ZAxisRotatingSpeed = 75f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * YAxisRotatingSpeed);
        transform.Rotate(Vector3.right * Time.deltaTime * XAxisRotatingSpeed);
        transform.Rotate(Vector3.forward * Time.deltaTime * ZAxisRotatingSpeed);
    }
}
