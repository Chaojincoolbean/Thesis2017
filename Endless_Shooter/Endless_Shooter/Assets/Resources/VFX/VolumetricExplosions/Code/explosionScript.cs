﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class explosionScript : MonoBehaviour {
	public float explosionForce;
	public float explosionRange;
	public bool Looping = false;
	public bool destroyOnEnd = false;
    public AudioClip[] explosionClips;

    AudioSource explosionSource;
    ParticleSystem [] allParticles;
	// Use this for initialization
	void  OnEnable() {
        explosionSource = GetComponent<AudioSource>();
        explosionSource.clip = explosionClips[Random.Range(0, explosionClips.Length)];
        explosionSource.Play();
		Rigidbody[] rb = GameObject.FindObjectsOfType<Rigidbody> ();
		foreach (Rigidbody r in rb) {
			r.AddExplosionForce (explosionForce, transform.position, explosionRange);
		}

		allParticles = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem p in allParticles) {
			var m = p.main;
			m.loop = Looping;
		}


		if (destroyOnEnd) {
			Destroy (this.gameObject, allParticles [0].main.duration);
		}


	}
	
	// Update is called once per frame
	void Update () {


	}


}
