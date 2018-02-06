namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class mannequin : MonoBehaviour
    {
        public GameObject[] drop;
        public float value = 20f;

        protected GameObject scoreManagement;
        protected float _health = 5f;
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
            scoreManagement = GameObject.Find("scoreManager");
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Die()
        {
            if (scoreManagement.GetComponent<scoreManager>() != null)
            {
                scoreManagement.GetComponent<scoreManager>().score -= value;
            }
            transform.GetComponent<Animator>().enabled = false;
            GameObject droppedItem = Instantiate(drop[Random.Range(0, drop.Length)], transform.position, Quaternion.identity) as GameObject;
        }
    }
}