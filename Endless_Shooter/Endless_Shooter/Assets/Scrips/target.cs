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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Die () {
		Instantiate (explosionVFX [Random.Range (0, explosionVFX.Count - 1)], gameObject.transform.position, gameObject.transform.rotation);
		explosionSource.clip = explosionSFX [Random.Range (0, explosionSFX.Count - 1)];
        Debug.Log(explosionSource.clip.name);
		explosionSource.Play ();
        Debug.Log(explosionSource.isPlaying);
		Destroy (gameObject);
	}
}
