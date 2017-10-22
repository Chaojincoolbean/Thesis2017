using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extendRib : MonoBehaviour {
	public GameObject[] ribs;
	public float spineCount = 2f;
	private float spinePosZ;
	private float reverseSpinePosZ;
	// Use this for initialization
	void Start () {
		spinePosZ = gameObject.transform.position.z + 50f;
		reverseSpinePosZ = gameObject.transform.position.z - 50f;
		for (int i = 0; i < spineCount; i++) {
			GameObject rib = Instantiate (ribs [Random.Range (0, ribs.Length)], new Vector3 (gameObject.transform.position.x, 0, spinePosZ), Quaternion.identity);
			spinePosZ = spinePosZ + 50f;
			rib.transform.parent = gameObject.transform;
		}
		for (int i = 0; i < spineCount; i++) {
			GameObject rib = Instantiate (ribs [Random.Range (0, ribs.Length)], new Vector3 (gameObject.transform.position.x, 0, reverseSpinePosZ), Quaternion.identity);
			reverseSpinePosZ = reverseSpinePosZ - 50f;
			rib.transform.parent = gameObject.transform;
		
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
