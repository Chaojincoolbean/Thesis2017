namespace VRTK
{
    using UnityEngine;
    using System.Collections;
    using DG.Tweening;
    using RootMotion.Dynamics;

    public class M1GarandFire : VRTK_InteractableObject
    {
        public GameObject muzzleFlash;
        public AudioClip[] rifleClip;
        public AudioClip[] grabSounds;
        public GameObject impactVFX;
        public GameObject brass;
        public GameObject enblocClip;
        public float magazineCapacity = 8f;
        public float range = 457f;
        public float damage = 1f;
        public float recoil = 1f;
        public float sway = 1f;
        public float ejectForce = 0.03f;
        public float unpin = 20f;
        public float force = 40000f;

        public Vector3 boltMoveBackPos;
        public Vector3 boltResetPos;

        private VRTK_ControllerEvents controllerEvents;
        private AudioSource rifleSource;
        public Transform bolt;
        public Transform muzzle;
        public Transform ejectWindow;
        //LineRenderer line;
        Rigidbody rb;
        // Use this for initialization
        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            rifleSource = gameObject.GetComponent<AudioSource>();
        }

        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
        {
            base.Grabbed(currentGrabbingObject);
            controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
            rifleSource.clip = grabSounds[Random.Range(0, grabSounds.Length)];
            rifleSource.Play();
            if (GetComponent<floatAndSpin>() != null)
            {
                GetComponent<floatAndSpin>().enabled = false;
            }
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
                GameObject spentCasing = Instantiate(brass, ejectWindow.transform.position, ejectWindow.transform.rotation) as GameObject;
                spentCasing.GetComponent<Rigidbody>().AddForce(-ejectWindow.transform.right * ejectForce, ForceMode.Impulse);
                spentCasing.transform.LookAt(muzzle);

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

                    if (Hit.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>() != null)
                    {
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

                Instantiate(impactVFX, Hit.point, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

            }
            else
            {
                //line.SetPosition(1, beamRay.GetPoint(100));
            }

            //Invoke("BeamOff", 0.1f);
            if (magazineCapacity > 1)
            {
                bolt.DOLocalMove(boltMoveBackPos, 0.05f).OnComplete(SlideRetract);
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
            rifleSource.clip = rifleClip[Random.Range(1,rifleClip.Length)];
            rifleSource.Play();
        }

        private void SlideRetract()
        {
            bolt.DOLocalMove(boltResetPos, 0.05f);
        }
        private void EmptyChamber()
        {
            Debug.Log("in");
            bolt.DOLocalMove(boltMoveBackPos, 0.05f);
            rifleSource.clip = rifleClip[0];
            rifleSource.Play();
            GameObject spentClip = Instantiate(enblocClip, ejectWindow.transform.position, ejectWindow.transform.rotation) as GameObject;
            spentClip.GetComponent<Rigidbody>().AddForce(-ejectWindow.transform.right * ejectForce, ForceMode.Impulse);
        }

        private void Destroy()
        {
            transform.DOScale(0, 0.5f);
            Destroy(gameObject, 0.7f);
        }
    }
}
