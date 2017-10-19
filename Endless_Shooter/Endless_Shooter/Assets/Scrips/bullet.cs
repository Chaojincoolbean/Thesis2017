using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {
	public float projectileSpeed = 50f;

	private Transform player;
	// Use this for initialization
	void Start () {
		//GameObject.Find<
		Invoke("TargetLockon", 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward*Time.deltaTime*projectileSpeed);
	}

	void TargetLockon () {
		player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
	}
}
