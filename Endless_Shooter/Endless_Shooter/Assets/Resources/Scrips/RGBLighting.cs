using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RGBLighting : MonoBehaviour {
    MeshRenderer meshRenderer;
    float f = 0;
    // Use this for initialization
    void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        
        f += Time.deltaTime;
        if (f >= 2.1f)
        {
            ChangeColor();
            f = 0;
        }
	}

    void ChangeColor()
    {
        Color randomColor = (Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f)*2);
        meshRenderer.material.DOColor(randomColor, "_EmissionColor", 2);
    }
}
