namespace VRTK.Examples {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using DG.Tweening;

		public class AK12Fire : VRTK_InteractableObject {
		public GameObject muzzleFlash;
		public AudioClip AK12SFX;
		public GameObject impactVFX;
		public float slowMutiplier = 0.7f;
        public float range = 200f;
        public float roundsPerMintue = 700;
		public float damage = 1f;

        private float timeToNextRound;
		private GameObject player;
		private GameObject world;
		private AudioSource AK12Source;
		private Transform bolt;
		private Transform muzzle;
		LineRenderer line;

		public override void StartUsing(VRTK_InteractUse usingObject) {
			base.StartUsing(usingObject);
            InvokeRepeating("FireRayCast", 0f, timeToNextRound);
			player = GameObject.Find("[VRTK_SDKManager]");
			if (player.GetComponent<move>() != null) 
			{
				player.GetComponent<move>().speed = player.GetComponent<move>().speed * slowMutiplier;
			}
		}

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            CancelInvoke();
			line.enabled = false;
        }
        // Use this for initialization
        void Start () {
            timeToNextRound = 60f / roundsPerMintue;
			bolt = gameObject.transform.GetChild (1).GetChild(0);
			muzzle = gameObject.transform.GetChild (0);
			AK12Source = gameObject.GetComponent<AudioSource> ();
			line = gameObject.GetComponent<LineRenderer> ();
			line.enabled = false;
			}

			private void FireRayCast () {
			print ("gun fired");
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
					Hit.collider.GetComponent<target> ().health -= damage;
					print(Hit.collider.GetComponent<target>().health);
				}

				Instantiate (impactVFX, Hit.point, Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360)));

			}
			else{
				line.SetPosition(1, beamRay.GetPoint(range));	
			} 

			Invoke ("BeamOff", 0.1f);

			bolt.DOLocalMoveX (0.6606456f, 0.07f);
			Invoke ("SlideRetract", 0.07f);
			Instantiate (muzzleFlash, muzzle.position, muzzle.rotation);
			AK12Source.clip = AK12SFX;
			AK12Source.Play ();
		}
	
		private void SlideRetract() {
			bolt.DOLocalMoveX (-0.6606456f, 0.07f);
		}

		public void BeamOff() {
			line.enabled = false;
		}
}
}
