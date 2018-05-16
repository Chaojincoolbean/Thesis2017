using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using RootMotion.Dynamics;
using DG.Tweening;
using System.Linq;

public class shotgunFire : VRTK_InteractableObject
{
    public GameObject muzzleFlash;
    public GameObject impactVFX;
    public GameObject brass;
    public Transform muzzle;
    public Transform ejectionPort;
    public AudioClip[] shotgunSFX;
    public AudioClip[] grabSounds;
    public int magazineCapacity = 8;
    public float damage = 1f;
    public int pelletCount = 9;
    public float spread = 0.5f;
    public float range;
    public float ejectForce;
    public float unpin = 20f;
    public float force = 30000f;
    public float recoil = 1f;
    public float sway = 0.5f;

    AudioClip[] impactClips;
    VRTK_ControllerEvents controllerEvents;
    Rigidbody rb;
    AudioSource source;

    public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
        source.clip = grabSounds[Random.Range(0, grabSounds.Length)];
        source.Play();
        if (GetComponent<floatAndSpin>() != null)
        {
            GetComponent<floatAndSpin>().enabled = false;
        }
    }

    public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        controllerEvents = null;
        if (magazineCapacity <= 0)
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

    // Use this for initialization
    void Start () {
        source = gameObject.GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody>();
        //impactClips = Resources.LoadAll("Bullets", typeof(AudioClip)).Cast<AudioClip>().ToArray();
    }

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        if (magazineCapacity > 0)
        {
            FireRayCast();
            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.63f, 0.2f, 0.01f);
            GameObject spentCasing = Instantiate(brass, ejectionPort.position, ejectionPort.rotation) as GameObject;
            spentCasing.GetComponent<Rigidbody>().AddForce(-ejectionPort.right * ejectForce, ForceMode.Impulse);
            spentCasing.transform.LookAt(muzzle);

            magazineCapacity -= 1;
        }
        else
        {
            StartCoroutine("Destroy");
            return;
        }

    }

    private void FireRayCast()
    {
        //print("gun fired");
        //line.enabled = true;
        //Vector3 pos = muzzle.position;
        //Ray beamRay = new Ray(pos, -muzzle.forward);

        //line.SetPosition(0, beamRay.origin);
        for (int i = 0; i < pelletCount; i++)
        {
            float spreadVariableX = Random.Range(-spread, spread);
            float spreadVariableY = Random.Range(-spread, spread);
            Vector3 randomSpread = new Vector3(spreadVariableX, spreadVariableY);
            Ray beamRay = new Ray(muzzle.position, - muzzle.forward + randomSpread);

            //print((-muzzle.forward + randomSpread).normalized + "From shotgun script");
            //print("Beam " + i + " Dir: " + beamRay.direction);

            RaycastHit Hit;
            
            Debug.DrawRay(muzzle.position, -muzzle.forward + randomSpread, Color.green, 82, true);

            if (Physics.Raycast(beamRay, out Hit, range))
            {
                
                if (Hit.collider.GetComponent<Rigidbody>() != null)
                {

                    Hit.collider.GetComponent<Rigidbody>().AddExplosionForce(5f, Hit.point, 3f, 0.5f, ForceMode.Impulse);

                    //Check if the rigidbody just got hit is a puppet muscle
                    if (Hit.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>() != null)
                    {
                        //Add physic force to puppet muscle
                        Hit.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>().Hit(unpin, beamRay.direction * force, Hit.point);
                        print(Hit.collider.name);

                        //Apply damage to the mannequin base script decided by where did player hit
                        if (Hit.collider.name == "Head")
                        {
                            Hit.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= damage * 5f;
                        }
                        else if (Hit.collider.name == "Chest")
                        {
                            Hit.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= damage * 3f;
                        }
                        else
                        {
                            Hit.collider.transform.parent.parent.GetChild(2).GetComponent<mannequinBase>().health -= damage;
                        }
                    }
                }

                if (Hit.collider.GetComponent<target>() != null)
                {
                    print(Hit.collider.name);
                    Hit.collider.GetComponent<target>().health -= damage;
                    print(Hit.collider.GetComponent<target>().health);
                }


                Instantiate(impactVFX, Hit.point, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

                /*if (Hit.collider.gameObject.tag == "PuppetLimb")
                {
                    AudioSource.PlayClipAtPoint(impactClips[Random.Range(47, 55)], Hit.point);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(impactClips[Random.Range(63, 72)], Hit.point);
                }*/

            }
            else
            {
                //line.SetPosition(1, beamRay.GetPoint(100));
            }
        }



        rb.AddForceAtPosition(muzzle.forward * recoil, muzzle.position);
        rb.AddForceAtPosition(muzzle.up * recoil, muzzle.position);
        rb.AddForceAtPosition(muzzle.right * sway, muzzle.position);
        GameObject fire = Instantiate(muzzleFlash, muzzle.position, muzzle.rotation) as GameObject;
        fire.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
        source.clip = shotgunSFX[Random.Range(0, shotgunSFX.Length)];
        source.Play();
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);
        transform.DOScale(0, 0.5f);
        Destroy(gameObject, 0.7f);
    }

}
