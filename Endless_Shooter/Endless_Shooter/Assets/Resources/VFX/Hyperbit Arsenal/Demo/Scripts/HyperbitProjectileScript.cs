using UnityEngine;
using System.Collections;
 
public class HyperbitProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [SerializeField] private GameObject playerCamera;
    public float damage = 10f;
    public Vector3 impactNormal; //Used to rotate impactparticle.
 
    private bool hasCollided = false;
 
    void Start()
    {
       projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
		if (muzzleParticle){
        GameObject spawnedMuzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
        Destroy(spawnedMuzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
        playerCamera = GameObject.Find("Camera (eye)");
        Destroy(gameObject, 30);
    }
 
    void OnCollisionEnter(Collision hit)
    {
        if (!hasCollided)
        {
            Impact();
 
            if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
            {
                Destroy(hit.gameObject);
            }
 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.name == "[VRTK][AUTOGEN][HeadsetColliderContainer]" || other.name == "[VRTK][AUTOGEN][BodyColliderContainer]")
        {
            print("Player Hit");
            playerCamera.GetComponent<playerHit>().PlayHitSound();
            playerCamera.GetComponent<playerHit>().CameraShake();
            playerCamera.GetComponent<playerHit>().PlayerHealthDecrease(damage);
        }
    }

    public void Impact()
    {
        hasCollided = true;
        //transform.DetachChildren();
        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

        foreach (GameObject trail in trailParticles)
        {
            GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
            curTrail.transform.parent = null;
            Destroy(curTrail, 3f);
        }
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 5f);
        Destroy(gameObject);

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
        //Component at [0] is that of the parent i.e. this object (if there is any)
        for (int i = 1; i < trails.Length; i++)
        {
            ParticleSystem trail = trails[i];
            if (!trail.gameObject.name.Contains("Trail"))
                continue;

            trail.transform.SetParent(null);
            Destroy(trail.gameObject, 2);
        }
    }

}