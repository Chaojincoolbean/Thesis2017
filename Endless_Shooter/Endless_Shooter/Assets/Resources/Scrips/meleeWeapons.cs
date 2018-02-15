using UnityEngine;
using System.Collections;
using VRTK;
using RootMotion.Dynamics;

public class meleeWeapons : VRTK_InteractableObject
{
    public MuscleRemoveMode removeMuscleMode;
    public float damage = 20f;
    VRTK_ControllerEvents controllerEvents;
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
    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        //If whatever stuff collide with this melee weapon has a rigidbody
        if (collision.collider.GetComponent<Rigidbody>() != null)
        {
            //If this rigidbody is a puppet muscle
            if (collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>() != null)
            {
                //Sever the limb
                var broadcaster = collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
                broadcaster.puppetMaster.RemoveMuscleRecursive(broadcaster.puppetMaster.muscles[broadcaster.muscleIndex].joint, true, true, removeMuscleMode);
                //Damage the mannequin enemy and print its remaining health
                collision.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= damage;
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
