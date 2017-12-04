namespace VRTK.Examples
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public enum State
    {
        fireSmallPellets, fireLargePellets
    }

    public class enemySoul : VRTK_InteractableObject
    {
        public List<AudioClip> fireSFX;
        public GameObject projectile;
        public float projectileForce = 100f;
        protected Transform muzzle;
        public float timeToNextRound=0.05f;
        protected AudioSource explosionSource;

        public State attackPattern;
        // Use this for initialization
        void Start()
        {
            muzzle = gameObject.transform.GetChild(1);
            explosionSource = gameObject.GetComponent<AudioSource>();
        }

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            switch (attackPattern)
            {
                case State.fireSmallPellets:
                    StartCoroutine("fireSmallPellets");
                    return;
                case State.fireLargePellets:
                    StartCoroutine("fireLargePellets");
                    return;
            }
        }

        IEnumerator fireSmallPellets()
        {

            float size = 0.2f;
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
            transform.DOScale(0, 0.5f);
            Destroy(gameObject, 0.7f);
        }

        IEnumerator fireLargePellets()
        {
            float size = 5f;
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
            transform.DOScale(0, 0.5f);
            Destroy(gameObject, 0.7f);
        }

    }
}