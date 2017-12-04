using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour {
	public float suicideTime = 1f;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, suicideTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
