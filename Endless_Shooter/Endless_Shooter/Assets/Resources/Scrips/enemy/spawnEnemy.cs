using UnityEngine;
using System.Collections;
using VRTK;

public class spawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawns;
    public float timeToNextSpawn;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 5f, timeToNextSpawn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy()
    {
        GameObject spwanedEnemy = Instantiate(enemy, spawns[Random.Range(0,spawns.Length-1)].position, Quaternion.identity) as GameObject;
        spwanedEnemy.transform.GetChild(2).GetComponent<mannequinMage>().projectileSpeed = Random.Range(500f, 3000f);
    }
}
