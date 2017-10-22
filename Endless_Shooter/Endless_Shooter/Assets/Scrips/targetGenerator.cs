using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetGenerator : MonoBehaviour {
	public GameObject[] targets;
	public float targetsNumber = 2f;
	public float targetMinHeight = 5f;
	private float targetsHeight;
	private float lotDimensionX;
	private float lotDimensionZ;
	private float lotPosX;
	private float lotPosZ;

	// Use this for initialization
	void Start () {
		targetsHeight = GameObject.Find ("Boundry1").GetComponent<buildingGenerator> ().buildingHeight;
		lotDimensionX = gameObject.transform.localScale.x;
		lotDimensionZ = gameObject.transform.localScale.z;
		lotPosX = gameObject.transform.position.x;
		lotPosZ = gameObject.transform.position.z;

		for (int i = 0; i <= targetsNumber; i++) {
			//Vector3 targetPos = new Vector3(
			float targetPosX = Random.Range(lotPosX-lotDimensionX/2, lotPosX+lotDimensionX/2);
			float targetPosZ = Random.Range (lotPosZ - lotDimensionZ / 2, lotPosZ + lotDimensionZ / 2);
			float targetPosY = Random.Range (targetMinHeight, targetsHeight);
			float targetLocalScale = Random.Range (5f, 20f);

			GameObject generatedTargets = Instantiate (targets[Random.Range(0, targets.Length)], gameObject.transform.position, Quaternion.identity);
			generatedTargets.transform.localPosition = new Vector3 (targetPosX, targetPosY, targetPosZ);
			generatedTargets.transform.localScale = new Vector3 (targetLocalScale, targetLocalScale, targetLocalScale);
			//generatedTargets.transform.parent = gameObject.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
