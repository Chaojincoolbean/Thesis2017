using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepOnLoad : MonoBehaviour {
    private keepOnLoad keepOnLoadInstance;
    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (keepOnLoadInstance == null)
        {
            keepOnLoadInstance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
