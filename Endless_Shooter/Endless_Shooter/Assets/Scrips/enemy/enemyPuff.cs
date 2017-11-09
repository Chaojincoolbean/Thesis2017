using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPuff : MonoBehaviour
{
    public GameObject enemy;
    public float enemyCount;

    private Transform muzzle;
    // Use this for initialization
    void Start()
    {
        muzzle = transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "[VRTK][AUTOGEN][HeadsetColliderContainer]")
        {
            StartCoroutine("SpawnEnemy");
        }
    }

    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemy, muzzle.position, Quaternion.identity);
            yield return new WaitForSeconds(.2f);
        }
    }
}