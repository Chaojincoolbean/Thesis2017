using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boumdry2Extension : MonoBehaviour {
    public GameObject wall;
    public float wallCount = 11f;
    private float wallPos;
    // Use this for initialization
    void Start () {
        wallPos = gameObject.transform.position.z - 50f;
        for (int i = 0; i < wallCount; i++)
        {
            GameObject spine = Instantiate(wall, new Vector3(gameObject.transform.position.x, 0, wallPos), Quaternion.identity);
            wallPos = wallPos - 50f;
            //GameObject.Find("buildingDensityManager").GetComponent<buildingDensityLog>().buildingDensity += 10f;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
