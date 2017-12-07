namespace VRTK.Examples
{
    using UnityEngine;
    using System.Collections;
    using DG.Tweening;

    public class M1911Fire : VRTK_InteractableObject
    {
        public GameObject muzzleFlash;
        public AudioClip M1911A1SFX;
        public GameObject impactVFX;
        public GameObject brass;
        public int magazineCapacity = 8;
        public float damage = 1f;
        public float range;
        public float ejectForce;

        public float recoil = 1f;
        public float sway = 0.5f;
        public Vector3 slideMoveBack;
		public Vector3 slideMoveForward;

        private VRTK_ControllerEvents controllerEvents;
        private AudioSource source;
        private Transform slide;
        private Transform muzzle;
        LineRenderer line;
        // Use this for initialization
        void Start()
        {
            slide = gameObject.transform.GetChild(1);
            muzzle = gameObject.transform.GetChild(3);
            source = gameObject.GetComponent<AudioSource>();
            /*line = gameObject.GetComponent<LineRenderer>();
            line.enabled = false;*/
        }

        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
        {
            base.Grabbed(currentGrabbingObject);
            controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
        }

        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
        {
            base.Ungrabbed(previousGrabbingObject);
            controllerEvents = null;
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
                }

                if (Hit.collider.GetComponent<target>() != null)
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
            }
            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(recoil, recoil, sway), muzzle.transform.position);
            Instantiate(muzzleFlash, muzzle.position, muzzle.rotation);
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
    }
}
