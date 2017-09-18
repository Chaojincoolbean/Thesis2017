using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour {
	public List<AudioClip> explosionSFX;
	public List<GameObject> explosionVFX;

	private AudioSource explosionSource;
	private float _health = 2f;
	public float health
	{
		get { return _health; }
		set 
		{ if (value <= 0f) {
				Die ();
			} else
				_health = value;
		}
	}
	// Use this for initialization
	void Start () {
		explosionSource = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Die () {
		Instantiate (explosionVFX [Random.Range (0, explosionVFX.Count - 1)], gameObject.transform.position, gameObject.transform.rotation);
		explosionSource.clip = explosionSFX [Random.Range (0, explosionSFX.Count - 1)];
		explosionSource.Play ();
		Destroy (gameObject);
	}
}
