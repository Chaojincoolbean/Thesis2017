using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extendSpine : MonoBehaviour {
	public GameObject[] spines;
	public float spineCount = 2f;
	private float spinePosX;
	private float reverseSpinePosX;
	// Use this for initialization
	void Start () {
		spinePosX = gameObject.transform.position.x + 50f;
		reverseSpinePosX = gameObject.transform.position.x - 50f;
		for (int i = 0; i < spineCount; i++) {
			GameObject spine = Instantiate (spines [Random.Range (0, spines.Length)], new Vector3 (spinePosX, 0, 0), Quaternion.identity);
			spinePosX = spinePosX + 50f;
		}
		for (int i = 0; i < spineCount; i++) {
			GameObject spine = Instantiate (spines [Random.Range (0, spines.Length)], new Vector3 (reverseSpinePosX, 0, 0), Quaternion.identity);
			reverseSpinePosX = reverseSpinePosX - 50f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
