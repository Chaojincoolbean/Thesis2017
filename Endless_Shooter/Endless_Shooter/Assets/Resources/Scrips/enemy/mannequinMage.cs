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

    public class mannequinMage : mannequinBase
    {
        public GameObject projectile;
        public GameObject muzzle;
        public States states;
        private int s;
        private float timer;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            s = Random.Range(0, States.GetNames(typeof(States)).Length - 1);
            print(s);
            states = (States)s;
            StateMachine(states);
   
        }

        // Update is called once per frame
        public override void Update()
        {
            //Decrease timer each frame, switch state once the timer reaches 0
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                s = Random.Range(0, States.GetNames(typeof(States)).Length - 1);
                print(s);
                states = (States)s;
                StateMachine(states);
            }
        }

        protected void StateMachine(States currentState)
        {
            switch (currentState)
            {
                case States.idle:
                    anim.SetBool("isMoving", false);
                    //Set a randomized timer to swich between cases
                    timer = Random.Range(1f, 3f);
                    break;
                case States.walk:
                    Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
                    anim.SetBool("isMoving", true);
                    transform.DORotateQuaternion(randomRotation, 0.5f);
                    //transform.rotation = randomRotation;
                    //transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
                    //Set a randomized timer to swich between cases
                    timer = Random.Range(1f, 3f);
                    break;
                case States.attack:

                    break;
            }
        }

    }
}
