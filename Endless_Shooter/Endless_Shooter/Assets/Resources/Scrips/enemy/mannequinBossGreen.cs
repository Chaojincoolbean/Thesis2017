﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using DG.Tweening;

public enum States
{
    idle, walk, attack
}

public enum SuperhumanChokeLiftPatterns
{
    bombMortar, bulletRadiance
}

public enum CastingSpellPatterns
{
    shootCluster
}

public class mannequinBossGreen : mannequinBase {
    public States states;
    public string[] triggerStrings;
    public GameObject mortarPos;
    public float projectileSpeed = 3000f;
    public float spreadFactor = 0.1f;

    private Ray ray;
    private bool isAttacking = false;
    private int s;
    private float timer;
    private bool isLostBalance = false;
    public SuperhumanChokeLiftPatterns liftPattern;
    public CastingSpellPatterns castingSpellPatterns;

    //Bullets used by this boss
    private GameObject greenBomb;
    private GameObject greenCluster;
    private GameObject greenFraction;
    private GameObject greenBullet;

    GameObject[] fractions;

    // Use this for initialization
    public override void Start () {
        base.Start();

        fractions = new GameObject[3];
        s = Random.Range(0, States.GetNames(typeof(States)).Length - 1);
        print(s);
        states = (States)s;
        StateMachine(states);
        int a = Random.Range(0, AttackPatterns.GetNames(typeof(AttackPatterns)).Length);
        //attackPattern = (AttackPatterns)a;

        greenBomb = Resources.Load("VFX/Hyperbit Arsenal/Demo/Prefabs/Projectiles/Fatbomb/FatBombMissileGreenOBJ") as GameObject;
        greenCluster = Resources.Load("VFX/Hyperbit Arsenal/Demo/Prefabs/Projectiles/ClusterBomb/ClusterBombGreenOBJ") as GameObject;
        greenFraction = Resources.Load("VFX/GeometryFXParticles/Prefabs/NaturePlayerEffects_Update1.1/FractionColorsEffects/GreenFraction") as GameObject;
        greenBullet = Resources.Load("VFX/Hyperbit Arsenal/Demo/Prefabs/Projectiles/Bullets/BulletGreenOBJ") as GameObject;
    }

    // Update is called once per frame
    public override void Update () {
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

        if (distanceToPlayer <= attackRange && isPlayerFound == true && isAttacking == false && isLostBalance == false && legsRemoved == false)
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
                print("Mannequin animation rootmotion State: " + anim.applyRootMotion);
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
                anim.SetTrigger(triggerStrings[Random.Range(0, triggerStrings.Length)]);
                //DOTween.Pause("RandomMovement");
                //transform.LookAt(player.transform.position);
                transform.DOLookAt(player.position, 0.5f, AxisConstraint.Y);
                break;
        }
    }

    public void AttackAnimationIsEnded()
    {
        isAttacking = false;
        /*if (magicTriangleMuzzles != null)
        {
            magicTriangleMuzzles[0].parent.gameObject.SetActive(false);
        }*/
    }

    public void SuperhumanChokeLift()
    {
        switch (liftPattern)
        {
            case SuperhumanChokeLiftPatterns.bombMortar:
                {
                    Vector3 flowerStem = transform.position + new Vector3(0, 2.5f, 0);
                    StartCoroutine(Mortar(flowerStem));
                }
                break;
            case SuperhumanChokeLiftPatterns.bulletRadiance:
                {
                    Vector3 fractionBase = transform.position - transform.right*2 + transform.up*3.5f;
                    StartCoroutine(BulletRadiance(fractionBase));
                }
                break;
        }
    }

    public void CastingSpell()
    {
        switch (castingSpellPatterns)
        {
            case CastingSpellPatterns.shootCluster:
                {

                }
                break;
        }
    }

    #region coroutines used in the SuperhumanChokeLift function
    IEnumerator Mortar(Vector3 muzzlePos)
    {
        for (int c = 0; c < 5; c++)
        {
            Vector3 flowerPos = muzzlePos + new Vector3(0, c * 1f, 0);
            for (int i = 0; i < 9; i++)
            {
                Quaternion pelletRotation = transform.rotation;
                pelletRotation.x += Random.Range(-spreadFactor, spreadFactor);
                pelletRotation.y += Random.Range(-spreadFactor, spreadFactor);
                pelletRotation.z += Random.Range(-spreadFactor, spreadFactor);
                GameObject pellet = Instantiate(greenBomb, flowerPos, pelletRotation);
                //pellet.transform.GetChild(0).GetComponent<AudioSource>().volume = 0.0625f;
                pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.up * projectileSpeed);
                pellet.GetComponent<Rigidbody>().useGravity = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator BulletRadiance(Vector3 fractionPos)
    {
        
        for(int i = 0; i< fractions.Length; i++)
        {
            if(fractions[i] == null)
            {
                GameObject newFraction = Instantiate(greenFraction, transform.position, transform.rotation);
                Destroy(newFraction, 2f);
                newFraction.transform.DOMove(fractionPos + transform.right*i*2 , 0.2f);
                fractions[i] = newFraction;
            }
            
        }
        
        while (fractions[0] != null)
        {
            float f = spreadFactor * 20f;
            for (int i = 0; i < fractions.Length; i++)
            {
                Quaternion pelletRotation = transform.rotation;
                pelletRotation.x += Random.Range(-f, f);
                pelletRotation.y += Random.Range(-f, f);
                pelletRotation.z += Random.Range(-f, f);
                GameObject pellet = Instantiate(greenBullet, fractions[i].transform.position, pelletRotation);
                pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.forward * projectileSpeed);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion

    #region coroutines used in the CastingSpell function

    #endregion
}
