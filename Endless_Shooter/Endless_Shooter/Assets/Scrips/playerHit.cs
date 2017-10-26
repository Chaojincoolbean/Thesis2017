using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playerHit : MonoBehaviour {
    public AudioClip playerHitClip;

    public float cameraShakeDuration = 0.5f;
    public float cameraShakeStrength = 5f;
    public int cameraVibrate = 5;
    public float cameraShakeRandomness = 40f;

    private Camera cam;
    private GameObject headColliderContainer;
    private AudioSource playerSource;
	// Use this for initialization
	void Start () {
        headColliderContainer = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]");
        cam = gameObject.GetComponent<Camera>();
        playerSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CameraShake()
    {
        cam.DOShakePosition(cameraShakeDuration, cameraShakeStrength, cameraVibrate, cameraShakeRandomness, true);
    }

    public void PlayHitSound()
    {
        playerSource.clip = playerHitClip;
        playerSource.Play();
    }
}
