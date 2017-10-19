using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour {
	public List<AudioClip> explosionSFX;
	public List<GameObject> explosionVFX;
    public GameObject projectile;
	public float value = 20;

	private AudioSource explosionSource;
	private GameObject scoreManagement;
	private Component managerOfScore;
    private Transform player;
	private float _health = 2f;
	public float health
	{
		get { return _health; }
		set 
		{
            _health = value;
            if (value <= 0f) {
				Die ();
			} 
				
		}
	}
	// Use this for initialization
	void Start () {
		explosionSource = gameObject.GetComponent<AudioSource> ();
		scoreManagement = GameObject.Find ("ScoreKeeper");
        Invoke("TargetLockon", 0.5f);
		//managerOfScore = scoreManagement.GetComponent<scoreManager> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null) {transform.LookAt(player);}
	}

	void Die () {
		Instantiate (explosionVFX [Random.Range (0, explosionVFX.Count - 1)], gameObject.transform.position, gameObject.transform.rotation);
		explosionSource.clip = explosionSFX [Random.Range (0, explosionSFX.Count - 1)];
        Debug.Log(explosionSource.clip.name);
		explosionSource.Play ();
        Debug.Log(explosionSource.isPlaying);

		if (scoreManagement.GetComponent<scoreManager> () != null) {
			scoreManagement.GetComponent<scoreManager> ().score += value;
		}

		Destroy (gameObject);
	}

    void TargetLockon()
    {
        player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
    }
}
