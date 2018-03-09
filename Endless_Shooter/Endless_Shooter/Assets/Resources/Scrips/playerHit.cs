using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using VRTK;

public class playerHit : MonoBehaviour {
    public AudioClip playerHitClip;
    public AudioClip deathClip;

    public float cameraShakeDuration = 0.5f;
    public float cameraShakeStrength = 5f;
    public int cameraVibrate = 5;
    public float cameraShakeRandomness = 40f;
    public float karmaMax = 100f;
    public float karmaMin = -100f;

    public float playerHealth = 100f;

    protected GameObject scoreManagement;
    private Camera cam;
    private GameObject headColliderContainer;
    private AudioSource playerSource;
    //private Rigidbody rb;
	// Use this for initialization
	void Start () {
        headColliderContainer = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]");
        cam = gameObject.GetComponent<Camera>();
        playerSource = gameObject.GetComponent<AudioSource>();
        scoreManagement = GameObject.Find("scoreManager");
        //rb = headColliderContainer.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        //Load scene depends on how much karma player has
        if (playerHealth < karmaMin)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
        if (playerHealth > karmaMax)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
        }
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
        playerHealth += value;
        if (scoreManagement.GetComponent<scoreManager>() != null)
        {
            scoreManagement.GetComponent<scoreManager>().score += value;
        }
        else
        {
            scoreManagement = GameObject.Find("scoreManager");
            scoreManagement.GetComponent<scoreManager>().score += value;
        }
    }

    public void PlayDeathSound()
    {
        playerSource.clip = deathClip;
        playerSource.Play();
    }
}
