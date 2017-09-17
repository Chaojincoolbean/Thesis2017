using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetGenerator : MonoBehaviour {
	public GameObject targets;
	public float targetsNumber = 2f;
	private float targetsHeight;
	// Use this for initialization
	void Start () {
		targetsHeight = GameObject.Find ("Block 1").GetComponent<buildingGenerator> ().buildingHeight;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
