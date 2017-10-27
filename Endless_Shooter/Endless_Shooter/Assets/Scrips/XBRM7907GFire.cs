namespace VRTK.Examples{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class XBRM7907GFire : VRTK_InteractableObject {
		public GameObject muzzleFlash;
		public float reservoirCapacity = 5;
        public float damage = 5f;
		public float reservoirRechargeTime = 0.01f;
		public float range = 300f;
		public AudioClip XBRM7907GFireclip;
		public GameObject beamImpactVFX;
        public float recoil = 1f;
        public float sway = 0.5f;

        private VRTK_ControllerEvents controllerEvents;
        private AudioSource source;
		private Transform muzzle;
		private float reservoir;
		LineRenderer line;
        Rigidbody rb;
        // Use this for initialization
        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject) {
            base.Grabbed(currentGrabbingObject);
            controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
        }

        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject) {
            base.Ungrabbed(previousGrabbingObject);
            controllerEvents = null;
        }

        public override void StartUsing (VRTK_InteractUse usingObject) {
			base.StartUsing(usingObject);
            FireRayCast();
            rb.AddForceAtPosition(new Vector3(recoil, recoil, sway), muzzle.transform.position);
            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.63f, 0.2f, 0.01f);

        }

		public override void StopUsing(VRTK_InteractUse usingObject){
			base.StopUsing(usingObject);
			//line.enabled = false;
			if (reservoir < reservoirCapacity) {
				reservoir += reservoirRechargeTime;
			} else if (reservoir > reservoirCapacity) {
				reservoir = reservoirCapacity;
			}
		}

	void Start () {
			reservoir = reservoirCapacity;
			muzzle = gameObject.transform.GetChild (0);
			source = gameObject.GetComponent<AudioSource> ();
			line = gameObject.GetComponent<LineRenderer> ();
			line.enabled = false;
            rb = gameObject.GetComponent<Rigidbody>();
	}
	
		private void FireRayCast () {
			//print ("gun fired");
			line.enabled = true;
			Vector3 pos = muzzle.position;

			Ray beamRay = new Ray (pos, -muzzle.forward);

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

				Instantiate (beamImpactVFX, Hit.point, Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360)));

			}
			else{
				line.SetPosition(1, beamRay.GetPoint(range));	
			} 

			Invoke ("BeamOff", 0.1f);

			//bolt.DOLocalMoveX (0.6606456f, 0.07f);
			//Invoke ("SlideRetract", 0.07f);
			GameObject flash = Instantiate (muzzleFlash, muzzle.position, muzzle.rotation) as GameObject;
            //flash.transform.rotation.Euler = flash.transform.rotation.Euler + new Vector3(0, 180, 0);
            source.clip = XBRM7907GFireclip;
			source.Play ();
		}
        public void BeamOff()
        {
            line.enabled = false;
        }
    }
}