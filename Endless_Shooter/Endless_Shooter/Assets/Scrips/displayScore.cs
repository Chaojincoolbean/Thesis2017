using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayScore : MonoBehaviour {
	private Text finalScoreText;
	private GameObject Manager;
	// Use this for initialization
	void Start () {
		Manager = GameObject.Find ("ScoreKeeper");

		finalScoreText = GameObject.Find ("Final Score").GetComponent<Text> ();
		finalScoreText.text = "Final Score: " + Manager.GetComponent<scoreManager> ().score;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
