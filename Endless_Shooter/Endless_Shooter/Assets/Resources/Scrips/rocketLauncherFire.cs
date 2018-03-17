using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using DG.Tweening;

public class rocketLauncherFire : VRTK_InteractableObject
{
    public AudioClip[] grabClips;
    public AudioClip fireClip;
    public GameObject[] rockets;
    public GameObject tailFlash;
    public Transform tailPipe;
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
        if (rocketIndex < rockets.Length)
        {
            if (GetComponent<floatAndSpin>() != null)
            {
                GetComponent<floatAndSpin>().enabled = false;
            }
        }
        else
        {
            if (GetComponent<floatAndSpin>() != null)
            {
                GetComponent<floatAndSpin>().enabled = true;
            }
        }
    }

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        if (rocketIndex < rockets.Length)
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
        if (rocketIndex > rockets.Length)
        {
            CancelInvoke();
            StartCoroutine("Destroy");
            return;
        }
        print(rocketIndex);
        print(rockets[rocketIndex]);
        rockets[rocketIndex].transform.position += rocketStartPos;
        rockets[rocketIndex].transform.parent = null;
        rockets[rocketIndex].AddComponent<Rigidbody>();
        rockets[rocketIndex].AddComponent<ConstantForce>();
        rockets[rocketIndex].AddComponent<rocket>();
        launcherSource.clip = fireClip;
        launcherSource.Play();
        GameObject flash = Instantiate(tailFlash, tailPipe.position, tailPipe.rotation);
        //flash.transform.LookAt(transform);
        rocketIndex += 1;

        VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 1);
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);
        transform.DOScale(0, 0.5f);
        Destroy(gameObject, 0.7f);
    }
}
