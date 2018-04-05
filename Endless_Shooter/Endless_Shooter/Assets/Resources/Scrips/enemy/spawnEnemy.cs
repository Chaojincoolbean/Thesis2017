using UnityEngine;
using System.Collections;
using VRTK;

public class spawnEnemy : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] spawnParticles;
    public Transform[] spawns;
    public float timeToNextSpawn;
    public int enemyCountLimit = 20;
    private bool enemyCountLimitReached = false;
    private GameObject[] enemiesInScene;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 5f, timeToNextSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        enemiesInScene = GameObject.FindGameObjectsWithTag("enemy");
        if (enemiesInScene.Length > 20)
        {
            enemyCountLimitReached = true;
        }
        else
        {
            enemyCountLimitReached = false;
        }
    }

    void SpawnEnemy()
    {
        if (enemyCountLimitReached == false)
        {
            GameObject spwanedEnemy = Instantiate(enemies[Random.Range(0,enemies.Length)], spawns[Random.Range(0, spawns.Length)].position, Quaternion.identity) as GameObject;
            spwanedEnemy.transform.GetChild(2).GetComponent<mannequinMage>().projectileSpeed = Random.Range(500f, 3000f);

            Vector3 spawnParticlePos = spwanedEnemy.transform.position + new Vector3(0, 1, 0);
            GameObject spawnParticle = Instantiate(spawnParticles[Random.Range(0, spawnParticles.Length)], spawnParticlePos, Quaternion.identity);
        }
    }
}
