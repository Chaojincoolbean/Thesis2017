using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class floatRandomly : MonoBehaviour {
	public float timeToNextDestination = 3f;
	public float range = 30f;
	public float speed = 10f;
	private Vector3 newDestination;
	private Rigidbody rb;
	float timer = 0;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		//InvokeRepeating ("TweenTo", 1f, timeToNextDestination);	
	}
	
	// Update is called once per frame
	void Update () {
		timer+=Time.deltaTime;
		if (timer > timeToNextDestination) {
			timer = 0;
			newDestination = getNewRandomPosition ();

		} else {
			transform.position = Vector3.MoveTowards (transform.position, newDestination, speed * Time.deltaTime);
		}
	}

	Vector3 getNewRandomPosition () {
		float x = Random.Range(-range, range);
		float z = Random.Range(-range, range);
		float y = Random.Range(0, 50f);

        if(transform.position.y > 50f)
        {
            transform.position = new Vector3(transform.position.x,50f,transform.position.z);
            y = 0;
        }

		Vector3 pos = new Vector3(x, y, z);
		return pos;
	}

	/*void TweenTo() {
		newDestination = getNewRandomPosition();
		gameObject.transform.DOMove (newDestination, timeToNextDestination - 0.1f, false);
	}

	void OnCollisionEnter (Collision collision) {
		DOTween.KillAll ();
	}*/
}
