namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public enum States
    {
        idle, walk, attack
    }

    public enum AttackPatterns
    {
        singleFire, consecutiveFire, rapidFire, multipleFire, beam
    }

    public enum SuperhumanChokeLiftPatterns
    {
        riseHolyBlades
    }

    public class mannequinMage : mannequinBase
    {
        public GameObject[] projectile;
        public GameObject[] waveAttackProjectiles;
        public GameObject[] fireBallProjectiles;
        public GameObject[] daggerProjectiles;
        public Transform[] magicTriangleMuzzles;
        public GameObject muzzle;
        public States states;
        public AttackPatterns attackPattern;
        public SuperhumanChokeLiftPatterns liftPattern;
        public float spreadFactor = 0.2f;
        public float projectileSpeed = 1000f;
        public int daggerCounts = 10;
        public string[] triggerStrings;

        private Ray ray;
        private GameObject beam;
        private GameObject beamStart;
        private GameObject beamEnd;
        private GameObject newBeamEnd;
        private GameObject newBeamStart;
        private GameObject newBeam;
        private GameObject magicTriangle;
        private LineRenderer line;
        private bool isAttacking = false;
        private int s;
        private float timer;
        private bool isLostBalance = false;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            s = Random.Range(0, States.GetNames(typeof(States)).Length - 1);
            print(s);
            states = (States)s;
            StateMachine(states);
            int a = Random.Range(0, AttackPatterns.GetNames(typeof(AttackPatterns)).Length);
            attackPattern = (AttackPatterns)a;

            //Load beam assets from Resources folder
            beam = Resources.Load("VFX/Hyperbit Arsenal/Prefabs/Beam/BasicBeam/LaserBeamYellow") as GameObject;
            beamStart = Resources.Load("VFX/Hyperbit Arsenal/Prefabs/Beam/BasicBeam/LaserBeamYellowStart") as GameObject;
            beamEnd = Resources.Load("VFX/Hyperbit Arsenal/Prefabs/Beam/BasicBeam/LaserBeamYellowEnd") as GameObject;

            /*if (magicTriangleMuzzles != null)
            {
                magicTriangle = magicTriangleMuzzles[0].parent.gameObject;
                magicTriangle.SetActive(false);
            }*/
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            //Decrease timer each frame, switch state once the timer reaches 0
            timer -= Time.deltaTime;
            //print(timer);
            if (timer <= 0 && isAttacking == false)
            {
                s = Random.Range(0, States.GetNames(typeof(States)).Length - 1);
                print(s);
                states = (States)s;
                StateMachine(states);
            }

            if (distanceToPlayer <= attackRange && isPlayerFound==true && isAttacking==false && isLostBalance==false && legsRemoved==false)
            {
                isAttacking = true;
                states = (States)2;
                StateMachine(states);
            }
            ray = new Ray(transform.position + new Vector3(0, 1.5f, 0), transform.forward);
            Physics.Raycast(ray, 100);
            Debug.DrawRay(transform.position + new Vector3(0, 1.5f, 0), transform.forward * 100, Color.red);

        }

        protected void StateMachine(States currentState)
        {
            switch (currentState)
            {
                case States.idle:
                    if (anim.applyRootMotion == false)
                    {
                        anim.applyRootMotion = true;
                    }
                    anim.SetBool("isMoving", false);
                    //Set a randomized timer to swich between cases
                    timer = Random.Range(1f, 3f);
                    break;
                case States.walk:
                    if (anim.applyRootMotion == false)
                    {
                        anim.applyRootMotion = true;
                    }
                    //print(anim.applyRootMotion);
                    Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
                    anim.SetBool("isMoving", true);
                    transform.DORotateQuaternion(randomRotation, 0.5f).SetId<Tweener>("RandomMovement");
                    //Set a randomized timer to swich between cases
                    timer = Random.Range(1f, 3f);
                    break;
                case States.attack:
                    //Player null check
                    if (!player)
                    {
                        player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
                    }
                    anim.applyRootMotion = false;
                    anim.SetTrigger(triggerStrings[Random.Range(0,triggerStrings.Length)]);
                    //DOTween.Pause("RandomMovement");
                    //transform.LookAt(player.transform.position);
                    transform.DOLookAt(player.position, 0.5f, AxisConstraint.Y);
                    break;
            }
        }

        public void AttackAnimationIsEnded()
        {
            isAttacking = false;  
            if (magicTriangleMuzzles != null)
            {
                magicTriangleMuzzles[0].parent.gameObject.SetActive(false);
            }
        }

        public void LaunchProjectile()
        {
            switch (attackPattern)
            {
                case AttackPatterns.singleFire:
                    {
                        GameObject launchedProjectile = Instantiate(projectile[0], muzzle.transform.position, muzzle.transform.rotation);
                        //launchedProjectile.transform.LookAt(player.transform);
                        launchedProjectile.GetComponent<Rigidbody>().AddForce(launchedProjectile.transform.forward * projectileSpeed);
                    }
                    break;
                case AttackPatterns.consecutiveFire:
                    {
                        StartCoroutine(Consecutive(3f, 0.2f));
                    }
                    break;
                case AttackPatterns.multipleFire:
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            Quaternion pelletRotation = muzzle.transform.rotation;
                            pelletRotation.x += Random.Range(-spreadFactor, spreadFactor);
                            pelletRotation.y += Random.Range(-spreadFactor, spreadFactor);
                            pelletRotation.z += Random.Range(-spreadFactor, spreadFactor);
                            GameObject pellet = Instantiate(projectile[2], muzzle.transform.position, pelletRotation);
                            //pellet.transform.GetChild(0).GetComponent<AudioSource>().volume = 0.0625f;
                            pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.forward * projectileSpeed);

                        }
                    }
                    break;
                case AttackPatterns.beam:
                    {
                        newBeamStart = Instantiate(beamStart, muzzle.transform.position, Quaternion.identity) as GameObject;
                        newBeamEnd = Instantiate(beamEnd, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        newBeam = Instantiate(beam, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        line = newBeam.GetComponent<LineRenderer>();
                        StartCoroutine(FireBeam());
                        StartCoroutine(DestroyBeam(0.5f));
                    }
                    break;
                case AttackPatterns.rapidFire:
                    {
                        StartCoroutine(Consecutive(10f, 0.05f));
                    }
                    break;
            }
        }

        IEnumerator Consecutive(float projectileCount, float timeToNextProjectile)
        {
            for (int i=1; i <= projectileCount; i++)
            {
                GameObject launchedProjectile = Instantiate(projectile[1], muzzle.transform.position, muzzle.transform.rotation);
                launchedProjectile.GetComponent<Rigidbody>().AddForce(launchedProjectile.transform.forward * projectileSpeed);
                //Debug.Log("firing bullet number " + i + " at time " + Time.time);
                //if (i == 3) Debug.Break();
                yield return new WaitForSeconds(timeToNextProjectile);
            }
        }

        IEnumerator FireBeam()
        {
            Ray beamRay = new Ray(muzzle.transform.position, muzzle.transform.forward);
            RaycastHit hit;
            line.SetPosition(0, beamRay.origin);

            if (Physics.Raycast(beamRay, out hit, 200f))
            {
                line.SetPosition(1, hit.point);
                newBeamEnd.transform.position = hit.point;
                if (hit.collider.name== "[VRTK][AUTOGEN][HeadsetColliderContainer]" || hit.collider.name == "[VRTK][AUTOGEN][BodyColliderContainer]")
                {
                    if (hit.collider.GetComponent<playerHit>() != null)
                    {
                        hit.collider.GetComponent<playerHit>().PlayerHealthDecrease(2f);
                    }
                }
            }
            else
            {
                line.SetPosition(1, beamRay.GetPoint(200f));
                newBeamEnd.transform.position = beamRay.GetPoint(200f);
            }
            newBeamStart.transform.LookAt(newBeamEnd.transform.position);
            newBeamEnd.transform.LookAt(newBeamStart.transform.position);
            yield return null;
        }

        IEnumerator DestroyBeam(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(newBeamStart);
            Destroy(newBeamEnd);
            Destroy(newBeam);
        }

        public IEnumerator WaveAttack()
        {
            float fasterProjectileSpeed = projectileSpeed * 1.5f;
            //Assign where should bullets come out from, which are left and right hand
            Transform leftHand = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
            Transform rightHand = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0);

            //Instantiate bullets and give them speed
            for (int i = 0; i < 6; i++)
            {
                GameObject launchedProjectileLeft = Instantiate(waveAttackProjectiles[Random.Range(0,waveAttackProjectiles.Length)], leftHand.position, muzzle.transform.rotation);
                launchedProjectileLeft.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * fasterProjectileSpeed);
                GameObject launchedProjectileRight = Instantiate(waveAttackProjectiles[Random.Range(0, waveAttackProjectiles.Length)], rightHand.position, muzzle.transform.rotation);
                launchedProjectileRight.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * fasterProjectileSpeed);
                yield return new WaitForSeconds(0.1f);
            }

            /*Print left hand and right hand to check if they are null
            print(leftHand);
            print(rightHand);*/
            
        }

        public IEnumerator LaunchFireBall()
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject launchedFireBall = Instantiate(fireBallProjectiles[Random.Range(0, fireBallProjectiles.Length)], muzzle.transform.position, muzzle.transform.rotation);
                launchedFireBall.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * projectileSpeed);
                yield return new WaitForSeconds(0.5f);
            }
        }

        public IEnumerator ThrowingDaggers()
        {
            for (int i = 0; i < daggerCounts; i++)
            {
                GameObject thrownDagger = Instantiate(daggerProjectiles[Random.Range(0, daggerProjectiles.Length)], muzzle.transform.position, muzzle.transform.rotation);
                thrownDagger.GetComponent<Rigidbody>().useGravity = false;
                thrownDagger.transform.LookAt(ray.GetPoint(100));
                thrownDagger.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed * 10);
                yield return new WaitForSeconds(0.02f);
            }
        }

        public IEnumerator MagicSpellCasting()
        {
            magicTriangleMuzzles[0].parent.gameObject.SetActive(true);
            //magicTriangleMuzzles[0].parent.LookAt(player.position);
            for (int i = 0; i < daggerCounts; i++)
            {
                GameObject thrownDagger1 = Instantiate(daggerProjectiles[Random.Range(0, daggerProjectiles.Length)], magicTriangleMuzzles[0].position, magicTriangleMuzzles[0].rotation);
                thrownDagger1.GetComponent<Rigidbody>().useGravity = false;
                thrownDagger1.GetComponent<Rigidbody>().AddForce(magicTriangleMuzzles[0].forward * projectileSpeed * 10);
                thrownDagger1.transform.rotation = Quaternion.LookRotation(thrownDagger1.GetComponent<Rigidbody>().velocity);

                GameObject thrownDagger2 = Instantiate(daggerProjectiles[Random.Range(0, daggerProjectiles.Length)], magicTriangleMuzzles[1].position, magicTriangleMuzzles[1].rotation);
                thrownDagger2.GetComponent<Rigidbody>().useGravity = false;
                thrownDagger2.GetComponent<Rigidbody>().AddForce(magicTriangleMuzzles[1].forward * projectileSpeed * 10);
                thrownDagger2.transform.rotation = Quaternion.LookRotation(thrownDagger2.GetComponent<Rigidbody>().velocity);

                GameObject thrownDagger3 = Instantiate(daggerProjectiles[Random.Range(0, daggerProjectiles.Length)], magicTriangleMuzzles[2].position, magicTriangleMuzzles[2].rotation);
                thrownDagger3.GetComponent<Rigidbody>().useGravity = false;
                thrownDagger3.GetComponent<Rigidbody>().AddForce(magicTriangleMuzzles[2].forward * projectileSpeed * 10);
                thrownDagger3.transform.rotation = Quaternion.LookRotation(thrownDagger3.GetComponent<Rigidbody>().velocity);
                yield return new WaitForSeconds(0.02f);
            }
            
        }

        public void SuperhumanChokeLift()
        {
            switch (liftPattern)
            {
                case SuperhumanChokeLiftPatterns.riseHolyBlades:
                    {
                        StartCoroutine(HolyBlades());
                    }
                    break;
            }

        }

        IEnumerator HolyBlades()
        {
            for (int i = 0; i < daggerCounts; i++)
            {
                float holyBladeSpreadFactor = spreadFactor * 6;
                Vector3 muzzleReference = transform.position + new Vector3(0, 1, -1.5f);
                muzzleReference.x += Random.Range(-holyBladeSpreadFactor, holyBladeSpreadFactor);
                //muzzleReference.y += Random.Range(-holyBladeSpreadFactor, holyBladeSpreadFactor);
                //muzzleReference.z += Random.Range(-holyBladeSpreadFactor, holyBladeSpreadFactor);

                Quaternion holyBladeRotation = transform.rotation;
                holyBladeRotation.x += Random.Range(-spreadFactor, spreadFactor);
                holyBladeRotation.y += Random.Range(-spreadFactor, spreadFactor);
                //holyBladeRotation.z += Random.Range(-spreadFactor, spreadFactor);

                GameObject holyBlade = Instantiate(daggerProjectiles[Random.Range(0, daggerProjectiles.Length)], muzzleReference, holyBladeRotation);
                //holyBlade.GetComponent<Rigidbody>().useGravity = false;
                holyBlade.GetComponent<Rigidbody>().AddForce(holyBlade.transform.up * projectileSpeed * 10);
                holyBlade.name = "Holy Blade" + i.ToString();
                //holyBlade.GetComponent<throwingKnifeBlade>().forceRotation = false;
                holyBlade.AddComponent<homing>();

                yield return new WaitForSeconds(0.02f);
            }
        }

        public void BalanceLost()
        {
            isLostBalance = true;
        }

        public void BalanceRegained()
        {
            isLostBalance = false;
        }
    }
}
