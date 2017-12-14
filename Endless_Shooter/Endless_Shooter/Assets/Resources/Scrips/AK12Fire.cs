namespace VRTK.Examples {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using DG.Tweening;

		public class AK12Fire : VRTK_InteractableObject {
		public GameObject muzzleFlash;
		public AudioClip[] rifleClip;
        public AudioClip[] grabSounds;
		public GameObject impactVFX;
        public GameObject brass;
        public float magazineCapacity = 30f;
        public float range = 200f;
        public float roundsPerMintue = 700;
		public float damage = 1f;
        public float recoil = 1f;
        public float sway = 1f;
        public float ejectForce = 0.03f;

        public Vector3 boltMoveBackPos;
        public Vector3 boltResetPos;

        private VRTK_ControllerEvents controllerEvents;
        private float timeToNextRound;
		private GameObject player;
		private GameObject world;
		private AudioSource AK12Source;
		public Transform bolt;
		public Transform muzzle;
        public Transform ejectWindow;
		//LineRenderer line;
        Rigidbody rb;

        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
        {
            base.Grabbed(currentGrabbingObject);
            controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
            AK12Source.clip = grabSounds[Random.Range(0, grabSounds.Length)];
            AK12Source.Play();
        }

        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
        {
            base.Ungrabbed(previousGrabbingObject);
            controllerEvents = null;
        }
   
        public override void StartUsing(VRTK_InteractUse usingObject) {
			base.StartUsing(usingObject);
            if (magazineCapacity > 0)
            {
                InvokeRepeating("FireRayCast", 0f, timeToNextRound);
            }
            else
            {
                CancelInvoke();
                SlideRetract();
            }


            /*player = GameObject.Find("[VRTK_SDKManager]");
			if (player.GetComponent<move>() != null) 
			{
				player.GetComponent<move>().speed = player.GetComponent<move>().speed * slowMutiplier;
			}*/
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            SlideRetract();
            CancelInvoke();
			//line.enabled = false;
        }
        // Use this for initialization
        void Start () {
            rb = gameObject.GetComponent<Rigidbody>();
            timeToNextRound = 60f / roundsPerMintue;
			AK12Source = gameObject.GetComponent<AudioSource> ();
			//line = gameObject.GetComponent<LineRenderer> ();
			//line.enabled = false;
			}

		private void FireRayCast () {
            //print ("gun fired");

            //line.enabled = true;
            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.63f);
            if (magazineCapacity <= 0)
            {
                CancelInvoke();
                return;
            }
			Vector3 pos = muzzle.position;

			Ray beamRay = new Ray (pos, -muzzle.forward);

			RaycastHit Hit;
			//line.SetPosition (0, beamRay.origin);

			if (Physics.Raycast(beamRay, out Hit, range))
			{
				//line.SetPosition(1, Hit.point);
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
				//line.SetPosition(1, beamRay.GetPoint(range));	
			}
            rb.AddForceAtPosition(muzzle.forward * recoil, muzzle.transform.position);
            rb.AddForceAtPosition(muzzle.up * recoil, muzzle.transform.position);
            rb.AddForceAtPosition(muzzle.right * sway, muzzle.transform.position);
            //Invoke ("BeamOff", 0.1f);

            bolt.DOLocalMove (boltMoveBackPos, 0.07f).OnComplete(SlideRetract);
			//Invoke ("SlideRetract", 0.07f);
			GameObject fire = Instantiate (muzzleFlash, muzzle.position, muzzle.rotation) as GameObject;
            fire.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.07f);

            GameObject spentCasing = Instantiate(brass, ejectWindow.position, muzzle.rotation) as GameObject;
            spentCasing.GetComponent<Rigidbody>().AddForce(-ejectWindow.right*ejectForce, ForceMode.Impulse);
            AK12Source.clip = rifleClip[Random.Range(1,rifleClip.Length-1)];
			AK12Source.Play ();
            magazineCapacity -= 1f;
		}
	
		private void SlideRetract() {
			bolt.DOLocalMove (boltResetPos, 0.07f);
            if (magazineCapacity > 0)
            {
                AK12Source.clip = rifleClip[0];
                AK12Source.Play();
            }
		}

		/*public void BeamOff() {
			line.enabled = false;
		}*/
}
}
