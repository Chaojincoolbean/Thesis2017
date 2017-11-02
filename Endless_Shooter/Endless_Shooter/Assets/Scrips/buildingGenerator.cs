using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class buildingGenerator : MonoBehaviour {
	public GameObject building;
	public float buildingNumbers = 20;
    public float buildingNumbersAfterStart = 30f;
	public float buildingLength = 3;
	public float buildingWidth = 3;
	public float buildingHeight = 10;

	float lotScaleX;
	float lotScaleY;
    GameObject generatedBuilding;
    // Use this for initialization
    void Start () {
        //buildingNumbers = GameObject.Find("buildingDensityManager").GetComponent<buildingDensityLog>().buildingDensity;
		lotScaleX = gameObject.transform.localScale.x;
		lotScaleY = gameObject.transform.localScale.z;

		for (int i = 1; i < buildingNumbers; i++) {
			generatedBuilding = Instantiate (building, gameObject.transform.position, Quaternion.identity);
			float buildPosX = Random.Range ((gameObject.transform.position.x-lotScaleX/2),(gameObject.transform.position.x+lotScaleX/2));
			float buildPosY = Random.Range ((gameObject.transform.position.z-lotScaleY/2),(gameObject.transform.position.z+lotScaleY/2));
            //generatedBuilding.transform.localScale = new Vector3 (Random.Range (0, buildingLength), Random.Range (0, buildingHeight), Random.Range (0, buildingWidth));
            generatedBuilding.transform.DOScale(new Vector3(Random.Range(0, buildingLength/ lotScaleX), Random.Range(0, buildingHeight), Random.Range(0, buildingWidth/ lotScaleY)), 0.5f);
			generatedBuilding.transform.localPosition = new Vector3 (buildPosX, generatedBuilding.transform.localScale.y/2, buildPosY);
			generatedBuilding.transform.parent = gameObject.transform;

		}
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TweenBuildings(buildingNumbersAfterStart);
        }
	}

    public void TweenBuildings(float buildingNumbersAfterStart)
    {
        for (int i = 1; i < buildingNumbersAfterStart; i++)
        {
            generatedBuilding = Instantiate(building, gameObject.transform.position, Quaternion.identity);
            float buildPosX = Random.Range((gameObject.transform.position.x - lotScaleX / 2), (gameObject.transform.position.x + lotScaleX / 2));
            float buildPosY = Random.Range((gameObject.transform.position.z - lotScaleY / 2), (gameObject.transform.position.z + lotScaleY / 2));
            //generatedBuilding.transform.localScale = new Vector3 (Random.Range (0, buildingLength), Random.Range (0, buildingHeight), Random.Range (0, buildingWidth));
            generatedBuilding.transform.DOScale(new Vector3(Random.Range(0, buildingLength / lotScaleX), Random.Range(0, buildingHeight), Random.Range(0, buildingWidth / lotScaleY)), 1f);
            //print(generatedBuilding.transform.localScale);
            generatedBuilding.transform.localPosition = new Vector3(buildPosX, generatedBuilding.transform.lossyScale.y / 2, buildPosY);
            generatedBuilding.transform.parent = gameObject.transform;

        }
        Invoke("GetThoseBuildingsInPlace", 0.6f);
    }

    void GetThoseBuildingsInPlace()
    {
        //print(generatedBuilding.transform.localScale);
        //GameObject.Find().transform.DOLocalMoveY(generatedBuilding.transform.localScale.y / 2, 0.5f);
        foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (gameObj.name == "Building(Clone)")
            {
                gameObj.transform.DOLocalMoveY(gameObj.transform.localScale.y / 2, 1f);
            }
        }
    }
}
