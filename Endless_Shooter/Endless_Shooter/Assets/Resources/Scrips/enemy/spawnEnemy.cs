using UnityEngine;
using System.Collections;
using VRTK;

public class spawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawns;
    public float timeToNextSpawn;
    public int enemyCountLimit = 20;
    private bool enemyCountLimitReached = false;
    private GameObject[] enemies;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 5f, timeToNextSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies.Length > 20)
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
            GameObject spwanedEnemy = Instantiate(enemy, spawns[Random.Range(0, spawns.Length)].position, Quaternion.identity) as GameObject;
            spwanedEnemy.transform.GetChild(2).GetComponent<mannequinMage>().projectileSpeed = Random.Range(500f, 3000f);
        }
    }
}
