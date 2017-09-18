using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideFollowScript : MonoBehaviour {

	GameObject PlayerCamera;
	public float RidePositionX;
	public float RidePositionY;
	public float RidePositionZ;

	// Use this for initialization
	void Start () {

		PlayerCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		
	}
	
	// Update is called once per frame
	void Update () {

		this.gameObject.transform.position = new Vector3 (PlayerCamera.transform.position.x + RidePositionX, 
			RidePositionY, PlayerCamera.transform.position.z + RidePositionZ);

		//this.gameObject.transform.rotation = new Quaternion(this.gameObject.transform.localRotation.x,
			//PlayerCamera.transform.rotation.
		
	}
}
