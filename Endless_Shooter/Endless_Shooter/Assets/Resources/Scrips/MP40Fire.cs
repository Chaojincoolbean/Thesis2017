 namespace VRTK.Examples {
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

	public class MP40Fire : VRTK_InteractableObject {

	// Use this for initialization
		public GameObject muzzleFlash;
		public AudioClip SmithWesson40calSFX;
		public GameObject impactVFX;
        public float slowMutiplier = 0.7f;

        private GameObject player;
        private GameObject world;
		private AudioSource MP40Source;
		private Transform slide;
		private Transform muzzle;
		LineRenderer line;

	public override void StartUsing(VRTK_InteractUse usingObject)
		{
			base.StartUsing(usingObject);
			FireRayCast();
            player = GameObject.Find("[VRTK_SDKManager]");
            if (player.GetComponent<move>() != null) 
            {
                player.GetComponent<move>().speed = player.GetComponent<move>().speed * slowMutiplier;
            }
		}
	protected void Start () {
			slide = gameObject.transform.GetChild (2);
			muzzle = gameObject.transform.GetChild (4);
			MP40Source = gameObject.GetComponent<AudioSource> ();
			line = gameObject.GetComponent<LineRenderer> ();
			line.enabled = false;
		}
	
	// Update is called once per frame
		private void FireRayCast () {
			print ("gun fired");
			line.enabled = true;
			Vector3 pos = muzzle.position;

			Ray beamRay = new Ray (pos, transform.up);

			RaycastHit Hit;
			line.SetPosition (0, beamRay.origin);

			if (Physics.Raycast(beamRay, out Hit, 100))
			{
				line.SetPosition(1, Hit.point);
				if (Hit.collider.GetComponent<Rigidbody>() != null)
				{
					Hit.collider.GetComponent<Rigidbody>().AddExplosionForce(5f, Hit.point, 3f, 2f, ForceMode.Impulse);
				}
                    
				if (Hit.collider.GetComponent<target> () != null) 
				{
                    print(Hit.collider.name);
					Hit.collider.GetComponent<target> ().health -= 1f;
                    print(Hit.collider.GetComponent<target>().health);
				}

				Instantiate (impactVFX, Hit.point, Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360)));

			}
			else{
				line.SetPosition(1, beamRay.GetPoint(100));	
			} 

			Invoke ("BeamOff", 0.1f);

			slide.DOLocalMoveY (-43f, 0.07f);
			Invoke ("SlideRetract", 0.07f);
			Instantiate (muzzleFlash, muzzle.position, muzzle.rotation);
			MP40Source.clip = SmithWesson40calSFX;
			MP40Source.Play ();
		}

		private void SlideRetract() {
			slide.DOLocalMoveY (3.147339f, 0.07f);
		}

		public void BeamOff() {
			line.enabled = false;
		}
	}
}
