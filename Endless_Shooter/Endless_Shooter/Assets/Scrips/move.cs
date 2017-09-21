namespace VRTK.Examples{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class move : MonoBehaviour {
	public float speed = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}
     
	public virtual void SwipeLeft (){
         print("Touchpad Swiped Left");
		gameObject.transform.DORotate (gameObject.transform.rotation.eulerAngles + new Vector3 (0, 90, 0), 0.5f);
	}

	public virtual void SwipeRight (){
         print("Touchpad Swiped Right");
		gameObject.transform.DORotate (gameObject.transform.rotation.eulerAngles + new Vector3 (0, -90, 0), 0.5f);
	}
}

}