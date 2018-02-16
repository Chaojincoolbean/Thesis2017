namespace VRTK.Examples
{
    using UnityEngine;
    using System.Collections;
    using DG.Tweening;
    using RootMotion.Dynamics;

    public class M1911Fire : VRTK_InteractableObject
    {
        public GameObject muzzleFlash;
        public AudioClip M1911A1SFX;
        public AudioClip[] grabSounds;
        public GameObject impactVFX;
        public GameObject brass;
        public int magazineCapacity = 8;
        public float damage = 1f;
        public float range;
        public float ejectForce;
        public float unpin = 20f;
        public float force = 30000f;

        public float recoil = 1f;
        public float sway = 0.5f;
        public Vector3 slideMoveBack;
		public Vector3 slideMoveForward;

        private VRTK_ControllerEvents controllerEvents;
        private AudioSource source;
        private Transform slide;
        private Transform muzzle;
        LineRenderer line;
        Rigidbody rb;
        // Use this for initialization
        void Start()
        {
            slide = gameObject.transform.GetChild(1);
            muzzle = gameObject.transform.GetChild(3);
            source = gameObject.GetComponent<AudioSource>();
            rb = gameObject.GetComponent<Rigidbody>();
            /*line = gameObject.GetComponent<LineRenderer>();
            line.enabled = false;*/
        }

        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
        {
            base.Grabbed(currentGrabbingObject);
            controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
            source.clip = grabSounds[Random.Range(0, grabSounds.Length)];
            source.Play();
        }

        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
        {
            base.Ungrabbed(previousGrabbingObject);
            controllerEvents = null;
            previousGrabbingObject.GetComponent<Rigidbody>().useGravity = enabled;
            previousGrabbingObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            if (magazineCapacity > 0)
            {
                FireRayCast();
                VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.63f, 0.2f, 0.01f);
                GameObject spentCasing = Instantiate(brass, slide.transform.position, slide.transform.rotation) as GameObject;
                spentCasing.GetComponent<Rigidbody>().AddForce(-slide.transform.right*ejectForce, ForceMode.Impulse);

                magazineCapacity -= 1;
            }
            else
            {
                return;
            }
            
        }

        private void FireRayCast()
        {
            //print("gun fired");
            //line.enabled = true;
            Vector3 pos = muzzle.position;

            Ray beamRay = new Ray(pos, -muzzle.transform.forward);

            RaycastHit Hit;
            //line.SetPosition(0, beamRay.origin);

            if (Physics.Raycast(beamRay, out Hit, range))
            {

                //line.SetPosition(1, Hit.point);
                if (Hit.collider.GetComponent<Rigidbody>() != null)
                {

                    Hit.collider.GetComponent<Rigidbody>().AddExplosionForce(5f, Hit.point, 3f, 2f, ForceMode.Impulse);

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
                if (Hit.collider.GetComponent<mannequinBase>() != null)
                {
                    print(Hit.collider.name);
                    Hit.collider.GetComponent<target>().health -= damage;
                    print(Hit.collider.GetComponent<target>().health);
                }

                Instantiate(impactVFX, Hit.point, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

            }
            else
            {
               //line.SetPosition(1, beamRay.GetPoint(100));
            }

            //Invoke("BeamOff", 0.1f);
            if (magazineCapacity > 1)
            {
                slide.DOLocalMove(slideMoveBack, 0.05f).OnComplete(SlideRetract);
            }
            else
            {
                EmptyChamber();
                Invoke("Destroy", 10f);
            }
            rb.AddForceAtPosition(muzzle.forward * recoil, muzzle.transform.position);
            rb.AddForceAtPosition(muzzle.up * recoil, muzzle.transform.position);
            rb.AddForceAtPosition(muzzle.right * sway, muzzle.transform.position);
            GameObject fire = Instantiate(muzzleFlash, muzzle.position, muzzle.rotation) as GameObject;
            fire.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
            source.clip = M1911A1SFX;
            source.Play();
        }

        private void SlideRetract()
        {
            slide.DOLocalMove(slideMoveForward, 0.05f);
        }
        private void EmptyChamber()
        {
            Debug.Log("in");
            slide.DOLocalMove(slideMoveBack, 0.05f);
        }
        public void BeamOff()
        {
            line.enabled = false;
        }
        private void Destroy()
        {
            transform.DOScale(0, 0.5f);
            Destroy(gameObject, 0.7f);
        }
    }
}
