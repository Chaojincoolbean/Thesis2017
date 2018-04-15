using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepFiring : MonoBehaviour {
    public GameObject bullet;
    public GameObject impactEffect;
    public float spreadFactor = 20f;
    public float projectileSpeed = 2000f;
    public Vector3 impactNormal;
    // Use this for initialization
    void Start () {
        Invoke("Impact", 10f);
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion pelletRotation = transform.rotation;
        pelletRotation.x += Random.Range(-spreadFactor, spreadFactor);
        pelletRotation.y += Random.Range(-spreadFactor, spreadFactor);
        pelletRotation.z += Random.Range(-spreadFactor, spreadFactor);
        GameObject pellet = Instantiate(bullet, transform.position, pelletRotation);
        pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.forward * projectileSpeed);
    }

    void OnCollisionEnter(Collision hit)
    {
        Impact();
    }

    void Impact()
    {
        GameObject spawnedImpactEffect = Instantiate(impactEffect, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));
        Destroy(spawnedImpactEffect, 10f);
        for (int i = 0; i < 20; i++)
        {
            Quaternion pelletRotation = transform.rotation;
            pelletRotation.x += Random.Range(-spreadFactor, spreadFactor);
            pelletRotation.y += Random.Range(-spreadFactor, spreadFactor);
            pelletRotation.z += Random.Range(-spreadFactor, spreadFactor);
            GameObject pellet = Instantiate(bullet, transform.position, pelletRotation);
            pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.forward * projectileSpeed);
        }
        Destroy(gameObject);
    }
}
