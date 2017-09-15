using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingGenerator : MonoBehaviour {
	public GameObject building;
	public int buildingNumbers = 5;
	public float buildingLength = 3;
	public float buildingWidth = 3;
	public float buildingHeight = 10;

	float lotScaleX;
	float lotScaleY;

	// Use this for initialization
	void Start () {
		lotScaleX = gameObject.transform.localScale.x;
		lotScaleY = gameObject.transform.localScale.z;

		for (int i = 1; i < buildingNumbers; i++) {
			GameObject generatedBuilding = Instantiate (building, gameObject.transform.position, Quaternion.identity);
			float buildPosX = Random.Range ((gameObject.transform.position.x-lotScaleX/2),(gameObject.transform.position.x+lotScaleX/2));
			float buildPosY = Random.Range ((gameObject.transform.position.z-lotScaleY/2),(gameObject.transform.position.z+lotScaleY/2));
			generatedBuilding.transform.localScale = new Vector3 (Random.Range (0, buildingLength), Random.Range (0, buildingHeight), Random.Range (0, buildingWidth));
			generatedBuilding.transform.localPosition = new Vector3 (buildPosX, generatedBuilding.transform.localScale.y/2, buildPosY);
			generatedBuilding.transform.parent = gameObject.transform;

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
