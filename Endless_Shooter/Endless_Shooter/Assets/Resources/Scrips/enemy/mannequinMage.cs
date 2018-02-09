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

        private bool isAttacking = false;
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
            base.Update();
            //Decrease timer each frame, switch state once the timer reaches 0
            timer -= Time.deltaTime;
            if (timer <= 0 && isAttacking == false)
            {
                s = Random.Range(0, States.GetNames(typeof(States)).Length - 1);
                print(s);
                states = (States)s;
                StateMachine(states);
            }

            if (distanceToPlayer <= attackRange && isAttacking==false)
            {
                isAttacking = true;
                states = (States)2;
                StateMachine(states);
            }

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
                    Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
                    anim.SetBool("isMoving", true);
                    transform.DORotateQuaternion(randomRotation, 0.5f).SetId<Tweener>("RandomMovement");
                    print("enter");
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
                    anim.SetTrigger("isAttacking");
                    DOTween.Pause("RandomMovement");
                    //transform.LookAt(player.transform.position);
                    transform.DOLookAt(player.position, 0.5f, AxisConstraint.Y);
                    break;
            }
        }

        public void AttackAnimationIsEnded()
        {
            isAttacking = false;
            
        }

    }
}
