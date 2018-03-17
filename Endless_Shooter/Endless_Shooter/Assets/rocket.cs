using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour {
    public float propellingForce;
    public float torque;

    GameObject trail;
    Object[] explosions;

    // Use this for initialization
    void Start () {
        explosions = Resources.LoadAll("VFX/VolumetricExplosions/Explosion Prefabs/RocketExplosions", typeof(GameObject));
        trail = Resources.Load("VFX/Rocket Flame Tail") as GameObject;

        GetComponent<ConstantForce>().force = transform.forward * 100;
        GetComponent<ConstantForce>().relativeTorque = transform.right * 5;

        Transform tail = transform.GetChild(0);
        Transform beacon = transform.GetChild(1);
        GameObject rocketTrail = Instantiate(trail, tail.position, tail.rotation) as GameObject;
        rocketTrail.transform.parent = transform;
        rocketTrail.transform.LookAt(beacon);

        print(explosions);
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Weapon")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(explosions[Random.Range(0, explosions.Length)], pos, rot);
            print("Rocket hit");
            Destroy(gameObject);
        }
    }
}
