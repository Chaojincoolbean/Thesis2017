using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnPrefab: MonoBehaviour {
	public GameObject[] explosionPrefab;
	public int SelectedExplosion = 0;
	Vector3 dMouse, mouse;

	public Text prefabName;


	void Start(){
		prefabName.text =  explosionPrefab [SelectedExplosion].name;
	}
	void Update () {

		Camera.main.transform.Translate(new Vector3(-Input.GetAxis("Vertical"),0,Input.GetAxis("Horizontal")), Space.World);

		if(Input.GetAxis("Mouse ScrollWheel") != 0){
			if(Input.GetAxis("Mouse ScrollWheel") < 0 && SelectedExplosion < explosionPrefab.Length - 1){
				SelectedExplosion = (SelectedExplosion + 1);

			}
			if(Input.GetAxis("Mouse ScrollWheel") > 0 && SelectedExplosion > 0){
				SelectedExplosion = (SelectedExplosion - 1);
			}  
			prefabName.text = explosionPrefab [SelectedExplosion].name;
		

		}


		if(Input.GetKeyDown(KeyCode.R)){
			SceneManager.LoadScene ("StreetExample");
		}

			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if ( Physics.Raycast (ray,out hit,1000f)) {
			
			if (Input.GetMouseButtonDown (0)) {	
				Instantiate (explosionPrefab[SelectedExplosion], hit.point, Quaternion.Euler(hit.normal));
			}

			if (Input.GetMouseButtonDown (1)) {	
				mouse = Input.mousePosition;
			}

			if (Input.GetMouseButton(1)) {	
				dMouse =  mouse - Input.mousePosition;
			}

			if (Input.GetMouseButtonUp (1)) {	
				mouse = dMouse = Vector3.zero;
			}

			if (Camera.main.transform.position.y >= 5f)
				Camera.main.transform.Translate (new Vector3 (0, 0, dMouse.y * 0.02f), Space.Self);
			else {
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, 5f, Camera.main.transform.position.z);
			}

			}
			



		}

	}


