using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {


	public float 		timer = 15f;
	public float 		timer2  = 3f;		


	public void Start()
	{
		Destroy(gameObject, timer);
	}

	
	public void OnCollisionEnter(Collision col) 
	{		
		Destroy (gameObject, timer2);
	}


}
