﻿namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using RootMotion.Dynamics;
    using DG.Tweening;
    using VRTK.GrabAttachMechanics;

    public class mannequinBase : MonoBehaviour
    {
        public GameObject[] drop;
        public float value = 20f;
        public float attackRange = 20f;
        public PuppetMaster puppetMaster;
        public BehaviourPuppet behaviourPuppet;
        public ConfigurableJoint[] leftLeg;
        public ConfigurableJoint[] rightLeg;

        GameObject puppetLimb;
        protected Transform player;
        protected Animator anim;
        protected GameObject scoreManagement;
        protected float distanceToPlayer;
        [SerializeField]protected float _health = 100f;
        protected bool isPlayerFound = false;
        protected GameObject playerCamera;
        protected bool dead = false;
        protected bool leftLegRemoved, rightLegRemoved;
        protected bool legsRemoved = false;
        protected int layerMask;
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
            puppetMaster.OnMuscleRemoved += OnMuscleRemoved;
            anim = GetComponent<Animator>();
            scoreManagement = GameObject.Find("scoreManager");
            Invoke("TargetLockon", 0.5f);
            layerMask = 1 << 4;

            //scoreManagement.GetComponent<scoreManager>().ignoreColliders.Add(puppetLimb);
            //VRTK_BodyPhysics currentBodyPhysics = GameObject.Find("PlayArea").GetComponent<VRTK_BodyPhysics>();
            //puppetLimb = puppetMaster.gameObject;
            //currentBodyPhysics.ignoreCollisionsWith = scoreManagement.GetComponent<scoreManager>().ignoreColliders.ToArray();
            //currentBodyPhysics.SendMessage("SetupIgnoredCollisions");
        }

        // Called by PM when a muscle is removed (once for each removed muscle)
        void OnMuscleRemoved(Muscle m)
        {
            bool isLeft = false;

            // If one of the legs is missing, play the "jump on one leg" animation. If both, set PM state to Dead.
            if (IsLegMuscle(m, out isLeft))
            {
                if (isLeft) leftLegRemoved = true;
                else rightLegRemoved = true;

                if (leftLegRemoved && rightLegRemoved)
                {
                    anim.SetTrigger("areLegsRemoved");
                    legsRemoved = true;
                }
            }
        }

        // Is the muscle a leg and if so, is it left or right?
        private bool IsLegMuscle(Muscle m, out bool isLeft)
        {
            isLeft = false;

            foreach (ConfigurableJoint j in leftLeg)
            {
                if (j == m.joint)
                {
                    isLeft = true;
                    return true;
                }
            }

            foreach (ConfigurableJoint j in rightLeg)
            {
                if (j == m.joint)
                {
                    isLeft = false;
                    return true;
                }
            }

            return false;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (player != null)
            {
                distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            }

            //print(distanceToPlayer);

            if (dead == false)
            {
                //Raycast to ground to see if this puppet should fall
                Ray groundRay = new Ray(transform.position + new Vector3(0, 1, 0), -transform.up);
                RaycastHit groundHit;
                Debug.DrawRay(transform.position + new Vector3(0, 1, 0), -transform.up * 1.2f, Color.green);
                if (Physics.Raycast(groundRay, out groundHit, 1.2f, layerMask))
                {
                    //print(groundHit.collider.gameObject.name);
                }
                else
                {
                    //print("Mannequin Fall");
                    behaviourPuppet.SetState(BehaviourPuppet.State.Unpinned);
                }
            }

        }

        void Die()
        {
            if (dead == false) //Alone with line 78, If this enemy has already dead it cannot "die again" and give player more points
            {
                if (scoreManagement.GetComponent<scoreManager>() != null)
                {
                    scoreManagement.GetComponent<scoreManager>().score -= value;
                    if (playerCamera != null)
                    {
                        playerCamera.GetComponent<playerHit>().playerHealth -= value;
                    }
                    else
                    {
                        playerCamera = GameObject.Find("Camera (eye)");
                        playerCamera.GetComponent<playerHit>().playerHealth -= value;
                    }
                }
                gameObject.transform.parent.GetChild(1).GetComponent<PuppetMaster>().Kill();

                if (drop.Length > 0)
                {
                    GameObject spawnManager = GameObject.Find("Enemy Spawn");
                    spawnManager.GetComponent<dropManager>().DropItem(transform.position);
                }

                Invoke("AssignVRGrabAttachMechanic", 3);

                dead = true;
            }
        }

        public void TargetLockon()
        {
            player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
            playerCamera = GameObject.Find("Camera (eye)");
            isPlayerFound = true;

            GameObject playerFoot = GameObject.Find("[VRTK][AUTOGEN][FootColliderContainer]");
            /*Collider[] ignorecolliders = puppetLimb.transform.GetComponentsInChildren<Collider>();
            foreach (Collider coll in ignorecolliders)
            {
                Physics.IgnoreCollision(coll,playerFoot.GetComponent<CapsuleCollider>(), true);
                Physics.IgnoreCollision(coll, GameObject.Find("PlayArea").GetComponent<VRTK_BodyPhysics>().GetFootColliderContainer().GetComponent<CapsuleCollider>());
            }*/
        }

        void AssignVRGrabAttachMechanic()
        {
            //Codes to Assign VRTK interacble object script to mannequin limbs when a mannequin dies
            GameObject[] mannequinLimbs = new GameObject[transform.parent.GetChild(1).childCount];
            for (int c = 0; c < transform.parent.GetChild(1).childCount; c++)
            {
                mannequinLimbs[c] = transform.parent.GetChild(1).GetChild(c).gameObject;
                //Add VRTK_InteractableObject and configer it
                mannequinLimbs[c].AddComponent<VRTK_InteractableObject>();
                mannequinLimbs[c].AddComponent<VRTK_TrackObjectGrabAttach>();
                mannequinLimbs[c].GetComponent<VRTK_InteractableObject>().touchHighlightColor = new Color(195, 255, 154, 255);
                mannequinLimbs[c].GetComponent<VRTK_InteractableObject>().isGrabbable = true;
                mannequinLimbs[c].GetComponent<VRTK_InteractableObject>().holdButtonToGrab = false;
                mannequinLimbs[c].GetComponent<VRTK_InteractableObject>().grabAttachMechanicScript = GetComponent<VRTK_TrackObjectGrabAttach>();
            }
        }
    }
}