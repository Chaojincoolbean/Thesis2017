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
        public int magazineCapacity = 8;
        public float damage = 1f;

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

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            if (magazineCapacity > 1)
            {
                FireRayCast();
                magazineCapacity -= 1;
            }
            if (magazineCapacity == 0)
            {
                EmptyChamber();   
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

            if (Physics.Raycast(beamRay, out Hit, 100))
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

            slide.DOLocalMove(new Vector3(0, 0.807f, -0.207f), 0.03f).OnComplete(SlideRetract);
            Invoke("SlideRetract", 0.07f);
            Instantiate(muzzleFlash, muzzle.position, muzzle.rotation);
            source.clip = M1911A1SFX;
            source.Play();
        }

        private void SlideRetract()
        {
            slide.DOLocalMove(new Vector3(0, 0.9273338f, -0.6073914f), 0.03f);
        }
        private void EmptyChamber()
        {
            slide.DOLocalMove(new Vector3(0, 0.807f, -0.207f), 0.03f);
        }
        public void BeamOff()
        {
            line.enabled = false;
        }
    }
}
