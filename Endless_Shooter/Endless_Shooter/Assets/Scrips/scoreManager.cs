using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scoreManager : MonoBehaviour {
	public float timerMaxValue = 180;
	public float timerMinValue = 90;
	public float score = 0;

	private float timeLeft;
	private Text timerText;
	//public Text finalScoreText;
	private scoreManager managerInstance;

	void Awake () {
		DontDestroyOnLoad (this);
		if (managerInstance == null) {
			managerInstance = this;
		} else {
			DestroyObject (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		timeLeft = Random.Range (timerMinValue, timerMaxValue);
		timerText = GameObject.Find ("Timer").GetComponent<Text> ();
		Invoke ("LoadGameOverScene", timeLeft + 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		timerText.text = "Time Left: " + (Mathf.Round (timeLeft));


	}

	public void LoadGameOverScene () {
		SceneManager.LoadScene (1, LoadSceneMode.Single);
		/*finalScoreText = GameObject.Find ("Final Score").GetComponent<Text> ();
		Debug.Log (finalScoreText.name);
		finalScoreText.text = "Final Score: " + score;*/
	}
}
