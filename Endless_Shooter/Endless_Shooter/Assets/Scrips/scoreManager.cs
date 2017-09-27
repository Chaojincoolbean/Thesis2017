using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {
	public float timerMaxValue = 180;
	public float timerMinValue = 90;
	public float score = 0;

	private float timeLeft;
	private Text timerText;

	void Awake () {
		
	}

	// Use this for initialization
	void Start () {
		timeLeft = Random.Range (timerMinValue, timerMaxValue);
		timerText = GameObject.Find ("Timer").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		timerText.text = "Time Left: " + (Mathf.Round (timeLeft));
		
	}
}
