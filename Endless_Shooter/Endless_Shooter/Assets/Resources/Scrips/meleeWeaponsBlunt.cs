using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using RootMotion.Dynamics;

public class meleeWeaponsBlunt : VRTK_InteractableObject
{
    public float damageMultiplier = 1f;
    public float damageTherehold = 20f;
    //public bool hasSpecialVFX = false;
    VRTK_ControllerEvents controllerEvents;
    public AudioClip[] bluntClips;
    public GameObject specialVFX;

    private AudioSource bluntSource;

    public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
    }

    public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        controllerEvents = null;
    }

    // Use this for initialization
    void Start () {
        bluntSource = GetComponent<AudioSource>();
	}

    void OnCollisionEnter(Collision collision)
    {
        //Check if this weapon has a special VFX to spawn one on collision
        if (specialVFX != null)
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            GameObject VFX = Instantiate(specialVFX, pos, rot);
            Destroy(VFX, 5f);
        }

        //If whatever stuff collide with this melee weapon has a rigidbody
        if (collision.collider.GetComponent<Rigidbody>() != null)
        {
            print(collision.relativeVelocity.magnitude + " From melee blunt and hit " + collision.collider.name);
            //If this rigidbody is a puppet muscle
            if (collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>() != null)
            {
                //Play hit SFX
                bluntSource.clip = bluntClips[Random.Range(0, bluntClips.Length)];
                bluntSource.Play();
                //Damage the mannequin enemy depend on where the player hit and print its remaining health
                //print(collision.collider.name);
                if (collision.relativeVelocity.magnitude > damageTherehold)
                {

                    if (collision.collider.name == "Head" || collision.collider.name == "Chest")
                    {
                        collision.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= collision.relativeVelocity.magnitude * damageMultiplier;
                    }
                    else
                    {
                        collision.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= collision.relativeVelocity.magnitude;
                    }

                    //Sever the limb
                    //var broadcaster = collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
                    //broadcaster.puppetMaster.RemoveMuscleRecursive(broadcaster.puppetMaster.muscles[broadcaster.muscleIndex].joint, true, true, removeMuscleMode);

                }
                else
                {
                    //collision.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= damage;
                }

                print(collision.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health);

            }
            else
            {
                // Not a muscle (any more)
                var joint = collision.collider.attachedRigidbody.GetComponent<ConfigurableJoint>();
                if (joint != null) Destroy(joint);
            }
        }
    }

}
