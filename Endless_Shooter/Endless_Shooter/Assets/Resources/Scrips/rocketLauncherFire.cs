using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using RootMotion.Dynamics;

public class rocketLauncherFire : VRTK_InteractableObject
{
    public AudioClip[] grabClips;
    public AudioClip[] explosionClips;
    public AudioClip launcherClip;
    public GameObject[] rockets;
    public Vector3 rocketStartPos;

    private VRTK_ControllerEvents controllerEvents;
    private AudioSource launcherSource;

    public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
        launcherSource.clip = grabClips[Random.Range(0, grabClips.Length)];
        launcherSource.Play();
        if (GetComponent<floatAndSpin>() != null)
        {
            GetComponent<floatAndSpin>().enabled = false;
        }
    }

    public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        controllerEvents = null;
        previousGrabbingObject.GetComponent<Rigidbody>().useGravity = enabled;
        previousGrabbingObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    // Use this for initialization
    void Start () {
        launcherSource = GetComponent<AudioSource>();
	}
	

}
