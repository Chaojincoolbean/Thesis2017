using UnityEngine;
using System.Collections;

public class spawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public float timeToNextSpawn;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Spawn", 5f, timeToNextSpawn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Spawn()
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
    }
}
