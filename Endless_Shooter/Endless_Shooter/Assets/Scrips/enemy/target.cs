namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using DG.Tweening;

    public class target : MonoBehaviour
    {
        public List<AudioClip> explosionSFX;
        public List<GameObject> explosionVFX;
        public List<AudioClip> fireSFX;
        public List<GameObject> drop;
        public GameObject projectile;
        public float value = 20;
        public float projectileForce = 100f;
        [SerializeField] protected float attackInterval;
        public float waitPeriod = 15f;
        public float roundsPerMintue = 5f;
        public string attackPattern;

        protected float timeToNextRound;
        protected AudioSource explosionSource;
        protected GameObject scoreManagement;
        //protected Component managerOfScore;
        protected Transform player;
        protected Transform muzzle;
        protected float f = 0f;
        protected float g = 0f;
        protected float _health = 2f;
        public float health
        {
            get { return _health; }
            set
            {
                _health = value;
                if (value <= 0f)
                {
                    Die();
                }

            }
        }
        // Use this for initialization
       public virtual void Start()
        {
            attackInterval = Random.Range(2f, 5f);
            explosionSource = gameObject.GetComponent<AudioSource>();
            scoreManagement = GameObject.Find("scoreManager");
            Invoke("TargetLockon", 0.5f);
            muzzle = gameObject.transform.GetChild(1);
            timeToNextRound = 60f / roundsPerMintue;
            //StartCoroutine(WaitAndGenerate(0.1f));
            //managerOfScore = scoreManagement.GetComponent<scoreManager> ();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            gameObject.transform.LookAt(player);
            g += Time.deltaTime;
			if ((SceneManager.GetActiveScene().name != "VR_City_Single block") && g >= waitPeriod)

            {
				//print ("correct scene");
				f += Time.deltaTime;
				//print (f);
				if (f >= attackInterval)
				{
					StartCoroutine (attackPattern);
					f = 0f;
					//print ("Coroutine Started");
				}

			}

        }

        void Die()
        {
            Instantiate(explosionVFX[Random.Range(0, explosionVFX.Count - 1)], gameObject.transform.position, gameObject.transform.rotation);
            Instantiate(drop[Random.Range(0, drop.Count - 1)], transform.position, Quaternion.identity);
            gameObject.transform.DOScale(0f, 0.1f);
            explosionSource.clip = explosionSFX[Random.Range(0, explosionSFX.Count - 1)];
            Debug.Log(explosionSource.clip.name);
            explosionSource.Play();
            Debug.Log(explosionSource.isPlaying);

            if (scoreManagement.GetComponent<scoreManager>() != null)
            {
                scoreManagement.GetComponent<scoreManager>().score += value;
            }

            Destroy(gameObject, 1f);
        }

        public void TargetLockon()
        {
            player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
        }

		IEnumerator FirePelletsAtPlayer()
        {

            float size = Random.Range(0.2f, 0.5f);
            float pellets = Random.Range(6f, 12f);
			for (int i = 1; i <= pellets; i++)
            {
                GameObject pellet = Instantiate(projectile, muzzle.position, Quaternion.identity) as GameObject;
                Rigidbody rb = pellet.GetComponent<Rigidbody>();
                //pellet.transform.parent = gameObject.transform;
                pellet.transform.localScale = new Vector3(size, size, size);
                rb.velocity = gameObject.transform.forward * projectileForce;

                explosionSource.clip = fireSFX[0];
                explosionSource.Play();
				yield return new WaitForSeconds(timeToNextRound);
            }


        }

        IEnumerator FireBigPellets()
        {
            float size = Random.Range(2f, 5f);
            float pellets = Random.Range(1f, 4f);
            for (int i = 1; i <= pellets; i++)
            {
                GameObject pellet = Instantiate(projectile, muzzle.position, Quaternion.identity) as GameObject;
                Rigidbody rb = pellet.GetComponent<Rigidbody>();
                //pellet.transform.parent = gameObject.transform;
                rb.velocity = gameObject.transform.forward * projectileForce;
                pellet.transform.localScale = new Vector3(size, size, size);
                explosionSource.clip = fireSFX[0];
                explosionSource.Play();
                yield return new WaitForSeconds(timeToNextRound);
            }

            
        }

    }
}
