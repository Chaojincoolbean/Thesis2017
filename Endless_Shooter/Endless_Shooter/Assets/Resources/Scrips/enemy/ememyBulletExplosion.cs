using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ememyBulletExplosion : MonoBehaviour {
    public float explosionForce;
    public float explosionRange;
    public float explosionDamage;

    private GameObject playerCamera;
    // Use this for initialization
    void Start () {
        playerCamera = GameObject.Find("Camera (eye)");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Rigidbody>() != null)
            {
                hitCollider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange, 2F, ForceMode.Impulse);
            }

            if (hitCollider.isTrigger == true)
            {
                if (hitCollider.name == "[VRTK][AUTOGEN][HeadsetColliderContainer]" | hitCollider.name == "[VRTK][AUTOGEN][BodyColliderContainer]")
                {
                    print("Player within ememy bullet explosion range");
                    playerCamera.GetComponent<playerHit>().PlayHitSound();
                    playerCamera.GetComponent<playerHit>().CameraShake();
                    playerCamera.GetComponent<playerHit>().PlayerHealthDecrease(explosionDamage);
                }
            }
        }

        
    }
	

}
