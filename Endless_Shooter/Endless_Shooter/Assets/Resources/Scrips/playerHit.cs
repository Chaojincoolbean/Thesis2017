using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class playerHit : MonoBehaviour {
    public AudioClip playerHitClip;
    public AudioClip deathClip;

    public float cameraShakeDuration = 0.5f;
    public float cameraShakeStrength = 5f;
    public int cameraVibrate = 5;
    public float cameraShakeRandomness = 40f;

    public float playerHealth = 100f;

    private Camera cam;
    private GameObject headColliderContainer;
    private AudioSource playerSource;
    //private Rigidbody rb;
	// Use this for initialization
	void Start () {
        headColliderContainer = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]");
        cam = gameObject.GetComponent<Camera>();
        playerSource = gameObject.GetComponent<AudioSource>();
        //rb = headColliderContainer.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerHealth <= 0f)
        {
            SceneManager.LoadScene("Game_Over", LoadSceneMode.Single);
            //Invoke("PlayDeathSound", 0.2f);
        }
        //print(VRPlayArea.GetComponent<>);
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

    public void PlayerHealthDecrease(float value)
    {
        playerHealth -= value;
    }

    public void PlayDeathSound()
    {
        playerSource.clip = deathClip;
        playerSource.Play();
    }
}
