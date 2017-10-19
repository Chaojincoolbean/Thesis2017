namespace VRTK.Examples{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class XBRM7907GFire : VRTK_InteractableObject {
		public GameObject muzzleFlash;
		public float magazineCapacity = 5;
		public float magazineRechargeTime = 0.5f;
		public float range = 300f;
		public AudioClip XBRM7907GFireclip;
		public GameObject beamImpactVFX;

		private AudioSource source;
		private Transform muzzle;
		LineRenderer line;
	// Use this for initialization

		public override void StartUsing (VRTK_InteractUse usingObject) {
			base.StartUsing(usingObject);
		}

	void Start () {
			muzzle = gameObject.transform.GetChild (0);
			source = gameObject.GetComponent<AudioSource> ();
			line = gameObject.GetComponent<LineRenderer> ();
			line.enabled = false;
	}
	
		private void FireRayCast () {
			//print ("gun fired");
			line.enabled = true;
			Vector3 pos = muzzle.position;

			Ray beamRay = new Ray (pos, transform.right);

			RaycastHit Hit;
			line.SetPosition (0, beamRay.origin);

			if (Physics.Raycast(beamRay, out Hit, range))
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

				Instantiate (beamImpactVFX, Hit.point, Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360)));

			}
			else{
				line.SetPosition(1, beamRay.GetPoint(range));	
			} 

			Invoke ("BeamOff", 0.1f);

			//bolt.DOLocalMoveX (0.6606456f, 0.07f);
			Invoke ("SlideRetract", 0.07f);
			Instantiate (muzzleFlash, muzzle.position, muzzle.rotation);
			source.clip = XBRM7907GFireclip;
			source.Play ();
		}
}
}