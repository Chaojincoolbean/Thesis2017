using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundry1Extension : MonoBehaviour {
    public GameObject wall;
    public float wallCount = 10f;
    private float wallPos;
    private float wallPosZ;
	// Use this for initialization
	void Start () {
        wallPos = gameObject.transform.position.x - 50f;
        wallPosZ = gameObject.transform.position.z - 50f;
        for (int i = 0; i < wallCount; i++)
        {
            GameObject spine = Instantiate(wall, new Vector3(wallPos, 0, gameObject.transform.position.z), Quaternion.identity);
            wallPos = wallPos - 50f;
            //GameObject.Find("buildingDensityManager").GetComponent<buildingDensityLog>().buildingDensity += 10f;
        }
        for (int i = 0; i < wallCount; i++)
        {
            GameObject spine = Instantiate(wall, new Vector3(gameObject.transform.position.x, 0, wallPosZ), Quaternion.identity);
            wallPosZ = wallPosZ - 50f;
            //GameObject.Find("buildingDensityManager").GetComponent<buildingDensityLog>().buildingDensity += 10f;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
