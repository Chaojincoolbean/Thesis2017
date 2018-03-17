using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using VRTK;

public class explosionScript : MonoBehaviour {
	public float explosionForce;
	public float explosionRange;
    public float explosionDamage;
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

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRange);
        foreach(Collider hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Rigidbody>() != null) //If colliders in this overlap sphere has a rigidbody
            {
                if (hitCollider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>() != null) //If this rigidbody is a puppet muscle
                {
                    hitCollider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= explosionDamage; //Damage the puppet within the explosion range
                    //Sever the limb
                    var broadcaster = hitCollider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
                    broadcaster.puppetMaster.RemoveMuscleRecursive(broadcaster.puppetMaster.muscles[broadcaster.muscleIndex].joint, true, true, MuscleRemoveMode.Explode);
                    hitCollider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange, 2F);
                }
                else
                {
                    // Not a muscle (any more)
                    var joint = hitCollider.attachedRigidbody.GetComponent<ConfigurableJoint>();
                    if (joint != null) Destroy(joint);
                }
            }
            else
            {
                hitCollider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange, 2F);
            }
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
