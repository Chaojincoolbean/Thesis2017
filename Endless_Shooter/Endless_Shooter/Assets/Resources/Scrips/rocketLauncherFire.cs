using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using RootMotion.Dynamics;

public class rocketLauncherFire : VRTK_InteractableObject
{
    public AudioClip[] grabClips;
    public AudioClip fireClip;
    public AudioClip launcherClip;
    public GameObject[] rockets;
    public Vector3 rocketStartPos;
    public float roundPerMintue = 900f;

    private VRTK_ControllerEvents controllerEvents;
    private AudioSource launcherSource;
    private int rocketIndex = 0;
    private float timeToNextRound;

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

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        if (rocketIndex<rockets.Length)
        {
            InvokeRepeating("FireRocket", 0f, timeToNextRound);
        }
        else
        {
            //isMagazineEmpty = true;
            CancelInvoke();
            StartCoroutine("Destroy");
        }
    }

    public override void StopUsing(VRTK_InteractUse usingObject)
    {
        base.StopUsing(usingObject);
        CancelInvoke();
        //line.enabled = false;
    }

    // Use this for initialization
    void Start () {
        launcherSource = GetComponent<AudioSource>();
        timeToNextRound = 60f / roundPerMintue;
    }
	
    void FireRocket()
    {
        rockets[rocketIndex].transform.position += rocketStartPos;
        rockets[rocketIndex].transform.parent = null;
        rockets[rocketIndex].AddComponent<Rigidbody>();
        rockets[rocketIndex].AddComponent<ConstantForce>();
        rockets[rocketIndex].AddComponent<rocket>();
        launcherSource.clip = fireClip;
        launcherSource.Play();
        rocketIndex += 1;
    }
}
