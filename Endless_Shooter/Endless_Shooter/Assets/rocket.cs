using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour {
    public float propellingForce;
    public float torque;

    GameObject trail;
    GameObject[] explosions = new GameObject[3];

    // Use this for initialization
    void Start () {
        explosions[0] = Resources.Load("VFX/VolumetricExplosions/21StylizedExplosion_Small") as GameObject;
        explosions[1] = Resources.Load("VFX/VolumetricExplosions/21StylizedExplosion_Small2") as GameObject;
        explosions[2] = Resources.Load("VFX/VolumetricExplosions/21StylizedExplosion_Small3") as GameObject;
        trail = Resources.Load("VFX/Rocket Flame Tail") as GameObject;

        GetComponent<ConstantForce>().force = transform.forward * 100;
        GetComponent<ConstantForce>().relativeTorque = transform.right * 5;

        Transform tail = transform.GetChild(0);
        GameObject rocketTrail = Instantiate(trail, tail.position, tail.rotation) as GameObject;
        rocketTrail.transform.parent = transform;
        //rocketTrail.transform.LookAt(gameObject.transform);
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        Instantiate(explosions[Random.Range(0,explosions.Length)], pos, rot);
        Destroy(gameObject);
    }
}
