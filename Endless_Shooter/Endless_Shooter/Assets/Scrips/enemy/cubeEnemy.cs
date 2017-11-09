namespace VRTK
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.SceneManagement;

    public class cubeEnemy : target
    {
        public float fireDistance = 10f;
        public Material bulletMat;
        public GameObject bulletHitSpark;

        public override void Start()
        {
            base.Start();
            attackInterval = 2f;
        }

        // Update is called once per frame
        public override void Update()
        {
            gameObject.transform.LookAt(player);
            g += Time.deltaTime;
            if ((SceneManager.GetActiveScene().name != "VR_City_Single block") && g >= waitPeriod)

            {
                //print ("correct scene");
                f += Time.deltaTime;
                //print (f);
                if (f >= attackInterval && Vector3.Distance(player.position, transform.position)<fireDistance)
                {
                    StartCoroutine(attackPattern);
                    f = 0f;
                    //print ("Coroutine Started");
                }

            }
        }

        IEnumerator FireEasyBulletsAtPlayer()
        {
            float size = Random.Range(0.4f, 0.8f);
            float pellets = Random.Range(1f, 4f);
            for (int i = 1; i <= pellets; i++)
            {
                GameObject pellet = Instantiate(projectile, muzzle.position, Quaternion.identity) as GameObject;
                Rigidbody rb = pellet.GetComponent<Rigidbody>();
                //pellet.transform.parent = gameObject.transform;
                pellet.transform.localScale = new Vector3(size, size, size);
                pellet.GetComponent<Renderer>().sharedMaterial = bulletMat;
                pellet.GetComponent<bullet>().magentaBulletHit = bulletHitSpark;
                rb.velocity = gameObject.transform.forward * projectileForce;

                explosionSource.clip = fireSFX[0];
                explosionSource.Play();
                yield return new WaitForSeconds(timeToNextRound);
            }
        }
    }
}