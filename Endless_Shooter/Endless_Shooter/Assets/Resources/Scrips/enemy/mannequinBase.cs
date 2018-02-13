namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using RootMotion.Dynamics;

    public class mannequinBase : MonoBehaviour
    {
        public GameObject[] drop;
        public float value = 20f;
        public float attackRange = 20f;

        protected Transform player;
        protected Animator anim;
        protected GameObject scoreManagement;
        protected float distanceToPlayer;
        protected float _health = 5f;
        protected bool isPlayerFound = false;
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
            anim = GetComponent<Animator>();
            scoreManagement = GameObject.Find("scoreManager");
            Invoke("TargetLockon", 0.5f);
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (player != null)
            {
                distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            }

            //print(distanceToPlayer);
        }

        void Die()
        {
            if (scoreManagement.GetComponent<scoreManager>() != null)
            {
                scoreManagement.GetComponent<scoreManager>().score -= value;
            }
            gameObject.transform.parent.GetChild(1).GetComponent<PuppetMaster>().Kill();
            GameObject droppedItem = Instantiate(drop[Random.Range(0, drop.Length)], transform.position, Quaternion.identity) as GameObject;
        }

        public void TargetLockon()
        {
            player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
            isPlayerFound = true;
        }
    }
}