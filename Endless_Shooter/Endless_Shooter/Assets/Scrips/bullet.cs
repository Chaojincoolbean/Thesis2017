using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bullet : MonoBehaviour {
	//public float projectileSpeed = 50f;
	public GameObject magentaBulletHit;
    public AudioClip bulletClip;

    [SerializeField] private AudioSource bulletSource;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerCamera;
	// Use this for initialization
	void Start () {
        //GameObject.Find<
        //Invoke("TargetLockon", 0.5f);
        //player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
        playerCamera = GameObject.Find("Camera (eye)");
		Invoke("SelfDestory", 3f);
        bulletSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Translate(Vector3.forward*Time.deltaTime*projectileSpeed);
	}

	/*void TargetLockon () {
		player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
	}*/

	void OnCollisionEnter (Collision col){
		ContactPoint contact = col.contacts [0];
		Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate (magentaBulletHit, pos, rot);
        if (!bulletSource)
        {
            Debug.Log(this.name+"1");
            bulletSource = GetComponent<AudioSource>();
        }
        bulletSource.clip = bulletClip;
        bulletSource.Play();
		Destroy (gameObject);
	}

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (other.name == "[VRTK][AUTOGEN][HeadsetColliderContainer]" || other.name == "[VRTK][AUTOGEN][BodyColliderContainer]")
        {
            playerCamera.GetComponent<playerHit>().PlayHitSound();
            playerCamera.GetComponent<playerHit>().CameraShake();
        }
    }

	void SelfDestory(){
		gameObject.transform.DOScale (0, 1);
		Destroy (gameObject, 1f);
	}
}
