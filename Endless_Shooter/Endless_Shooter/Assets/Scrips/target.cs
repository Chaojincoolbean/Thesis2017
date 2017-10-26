﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour {
	public List<AudioClip> explosionSFX;
	public List<GameObject> explosionVFX;
    public List<AudioClip> fireSFX;
    public GameObject projectile;
	public float value = 20;
    public float projectileForce = 100f;
    public float attackInterval;
    public float waitPeriod = 15f;

	private AudioSource explosionSource;
	private GameObject scoreManagement;
	private Component managerOfScore;
    private Transform player;
    private Transform muzzle;
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
        attackInterval = Random.Range(0.5f, 3f);
		explosionSource = gameObject.GetComponent<AudioSource> ();
		scoreManagement = GameObject.Find ("ScoreKeeper");
        Invoke("TargetLockon", 0.5f);
        muzzle = gameObject.transform.GetChild(1);
        InvokeRepeating("FireAtPlayer", waitPeriod, attackInterval);
        //StartCoroutine(WaitAndGenerate(0.1f));
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

    public void TargetLockon()
    {
        player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
    }

    void FireAtPlayer()
    {
        
        float size = Random.Range(0.2f, 1f);
        float pellets = Random.Range(6f, 12f);
		for (int i = 1; i <= 1; i++)
        {
            GameObject pellet = Instantiate(projectile, muzzle.position, Quaternion.identity) as GameObject;
            Rigidbody rb = pellet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileForce);
            explosionSource.clip = fireSFX[0];
            explosionSource.Play();
			//pellet.transform.Translate(Vector3.forward*Time.deltaTime*projectileForce);
            pellet.transform.localScale = new Vector3(size, size, size);
        }


    }

    // every 2 seconds perform the print()
    /*private IEnumerator WaitAndGenerate(float waitTime)
    {
        float size = Random.Range(0.2f, 1f);
        float pellets = Random.Range(6f, 12f);
        for (int i = 1; i <= 5; i++)
        {
            GameObject pellet = Instantiate(projectile, muzzle.position, Quaternion.identity) as GameObject;
            Rigidbody rb = pellet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileForce);
            explosionSource.clip = fireSFX[0];
            explosionSource.Play();
            Debug.Log("in");
            //pellet.transform.Translate(Vector3.forward*Time.deltaTime*projectileForce);
            pellet.transform.localScale = new Vector3(size, size, size);
            yield return new WaitForSeconds(waitTime);
            //print("WaitAndPrint " + Time.time);
        }
    }*/
}
