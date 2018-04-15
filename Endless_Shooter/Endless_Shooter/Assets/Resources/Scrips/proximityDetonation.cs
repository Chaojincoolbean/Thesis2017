using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximityDetonation : MonoBehaviour {
    public float detonationDistance = 2f;
    public float sharpenalCount = 10f;
    public float sharpenalForce = 2000f;
    public GameObject sharpenal;

    private Transform player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
    }
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(player.position, transform.position) < detonationDistance)
        {
            Detonate();
            if (gameObject.GetComponent<HyperbitProjectileScript>() != null)
            {
                gameObject.GetComponent<HyperbitProjectileScript>().Impact();
            }
        }
	}

    void Detonate()
    {
        print("Proximity Fuse Triggered");
        float spreadFactor = 20f;
        for (int i = 0; i < sharpenalCount; i++)
        {
            Quaternion pelletRotation = transform.rotation;
            pelletRotation.x += Random.Range(-spreadFactor, spreadFactor);
            pelletRotation.y += Random.Range(-spreadFactor, spreadFactor);
            pelletRotation.z += Random.Range(-spreadFactor, spreadFactor);
            GameObject pellet = Instantiate(sharpenal, transform.position, pelletRotation);
            pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.forward * sharpenalForce);
        }
    }
}
