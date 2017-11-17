using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraDepth : MonoBehaviour {
    Camera cam;
	// Use this for initialization
	void Start () {
        cam = gameObject.GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.DepthNormals;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
