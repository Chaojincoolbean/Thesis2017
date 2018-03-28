using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemTest : MonoBehaviour {
    public GameObject particle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
        {
            particle = Instantiate(particle, transform.position, transform.rotation);
            Destroy(particle, 1.5f);
        }
	}
}
