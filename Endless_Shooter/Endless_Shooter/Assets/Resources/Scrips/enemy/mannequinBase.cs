namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using RootMotion.Dynamics;
    using DG.Tweening;

    public class mannequinBase : MonoBehaviour
    {
        public GameObject[] drop;
        public float value = 20f;
        public float attackRange = 20f;
        public PuppetMaster puppetMaster;
        public ConfigurableJoint[] leftLeg;
        public ConfigurableJoint[] rightLeg;

        GameObject puppetLimb;
        protected Transform player;
        protected Animator anim;
        protected GameObject scoreManagement;
        protected float distanceToPlayer;
        protected float _health = 100f;
        protected bool isPlayerFound = false;
        protected GameObject playerCamera;
        protected bool dead = false;
        protected bool leftLegRemoved, rightLegRemoved;
        protected bool legsRemoved = false;
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
            VRTK_BodyPhysics currentBodyPhysics = GameObject.Find("PlayArea").GetComponent<VRTK_BodyPhysics>();
            puppetLimb = puppetMaster.gameObject;
            scoreManagement.GetComponent<scoreManager>().ignoreColliders.Add(puppetLimb);
            currentBodyPhysics.ignoreCollisionsWith = scoreManagement.GetComponent<scoreManager>().ignoreColliders.ToArray();
            currentBodyPhysics.SendMessage("SetupIgnoredCollisions");
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
                    GameObject droppedItem = Instantiate(drop[Random.Range(0, drop.Length)], transform.position, Quaternion.identity) as GameObject;
                    //droppedItem.GetComponent<Rigidbody>().useGravity = false;
                    droppedItem.GetComponent<Rigidbody>().isKinematic = true;
                    droppedItem.transform.DOMoveY(droppedItem.transform.position.y + 1f, 0.5f);
                }

                dead = true;
            }
        }

        public void TargetLockon()
        {
            player = GameObject.Find("[VRTK][AUTOGEN][HeadsetColliderContainer]").transform;
            playerCamera = GameObject.Find("Camera (eye)");
            isPlayerFound = true;

            GameObject playerFoot = GameObject.Find("[VRTK][AUTOGEN][FootColliderContainer]");
            Collider[] ignorecolliders = puppetLimb.transform.GetComponentsInChildren<Collider>();
            foreach (Collider coll in ignorecolliders)
            {
                Physics.IgnoreCollision(coll,playerFoot.GetComponent<CapsuleCollider>(), true);
                Physics.IgnoreCollision(coll, GameObject.Find("PlayArea").GetComponent<VRTK_BodyPhysics>().GetFootColliderContainer().GetComponent<CapsuleCollider>());
            }
        }
    }
}