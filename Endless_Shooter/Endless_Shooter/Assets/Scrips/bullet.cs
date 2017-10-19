using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bullet : MonoBehaviour {
	//public float projectileSpeed = 50f;
	public GameObject magentaBulletHit;

	private Transform player;
	// Use this for initialization
	void Start () {
		//GameObject.Find<
		//Invoke("TargetLockon", 0.5f);
		//player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
		Invoke("SelfDestory", 3f);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Translate(Vector3.forward*Time.deltaTime*projectileSpeed);
	}

	/*void TargetLockon () {
		player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
	}*/

	void OnCollisionEnter (Collision col){
		ContactPoint contact = col.contacts [0];
		Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate (magentaBulletHit, pos, rot);
		Destroy (gameObject);
	}

	void SelfDestory(){
		gameObject.transform.DOScale (0, 1);
		Destroy (gameObject);
	}
}
