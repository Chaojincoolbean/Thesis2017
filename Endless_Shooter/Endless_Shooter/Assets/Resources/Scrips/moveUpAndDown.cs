using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveUpAndDown : MonoBehaviour {
    public float speed = 10f;
    public float timeToNextPosition = 10f;

    float f = 8f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        f += Time.deltaTime;
        print(f);
        if (f >= timeToNextPosition)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
            Invoke("MoveDown", 5f);
            f = 0;
        }
        
	}

    void MoveDown()
    {
        transform.Translate(-Vector3.up * Time.deltaTime * speed);
    }
}
