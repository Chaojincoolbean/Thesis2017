using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Grenade : VRTK_InteractableObject
{
    public float fuseTimer = 5f;
    public AudioClip[] grabSounds;
    public AudioClip fuseSound;
    public GameObject nozzle;

    VRTK_ControllerEvents controllerEvents;
    AudioSource source;
    GameObject smoke;
    Object[] explosions;
    bool isFuseBurning = false;

    public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
        source.clip = grabSounds[Random.Range(0, grabSounds.Length)];
        source.Play();
    }

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        if (isFuseBurning == false)
        {
            GameObject emittedSmoke = Instantiate(smoke, nozzle.transform.position, nozzle.transform.rotation) as GameObject;
            emittedSmoke.transform.parent = nozzle.transform;
            StartCoroutine("PullTheString");
            source.clip = fuseSound;
            source.Play();
            source.loop = true;
            isFuseBurning = true;
        }
    }

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
        smoke = Resources.Load("VFX/GrenadeSmoke") as GameObject;
        explosions = Resources.LoadAll("VFX/VolumetricExplosions/Explosion Prefabs/RocketExplosions", typeof(GameObject));
    }
	
    IEnumerator PullTheString()
    {
        yield return new WaitForSeconds(fuseTimer);
        Instantiate(explosions[Random.Range(0, explosions.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
