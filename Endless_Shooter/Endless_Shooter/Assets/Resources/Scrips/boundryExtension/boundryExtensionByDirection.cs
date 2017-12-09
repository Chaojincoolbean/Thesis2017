using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class boundryExtensionByDirection : MonoBehaviour {
	public GameObject wall;
	public int wallCount = 1;
	public float x = 25f;
	public float y = 25f;
	public float z = 25f;
	// Use this for initialization
	void Start () {
		//int length = 5;
		Vector3 pos = new Vector3(x,y,z);
		for (int i = 1; i <= wallCount; i++) {
			Instantiate (wall, gameObject.transform.localPosition+pos, Quaternion.identity);
			//length += 1;
			pos += new Vector3(x,y,z);
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)){
            Vector3 pos = new Vector3(x, y, z);
            for (int i = 1; i <= wallCount; i++)
            {
                Instantiate(wall, gameObject.transform.localPosition + pos, Quaternion.identity);
                //length += 1;
                pos += new Vector3(x, y, z);
            }
        }
    }
}
