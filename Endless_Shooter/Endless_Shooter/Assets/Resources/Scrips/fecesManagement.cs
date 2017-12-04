using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class fecesManagement : MonoBehaviour {
    [SerializeField]private GameObject[] slogans;
    public float countdown = 7f;
    public Camera cam;
    private int index;
    private GameObject playerBody;
    private GameObject playArea;
    public GameObject anus;
    public GameObject[] shit;
    public Material[] gradients;
    public GameObject asshole;
    public GameObject butthole;
    float c = 0;
    // Use this for initialization
    void Start () {
        index = Random.Range(0, slogans.Length - 1);
        DisplaySlogan(index);
        Invoke("DisableSlogan", countdown - 1f);
        Invoke("FindPlayer", 0.1f);

    }
	
	// Update is called once per frame
	void Update () {
        c += Time.deltaTime;
        //print(c);
        if (c >= countdown)
        {
            //print("engaged");
            index = Random.Range(0, slogans.Length - 1);
            DisplaySlogan(index);
            Invoke("DisableSlogan", countdown - 1f);
            c = 0f;
        }
        asshole.GetComponent<ParticleSystem>().startSize = Random.Range(1, 5);
        butthole.GetComponent<ParticleSystem>().startSize = Random.Range(1, 5);
    }

    void DisplaySlogan(int i)
    {
        slogans[i].SetActive(true);
        Color randomColor = (Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f));
        cam.DOColor(randomColor, 1f);
        //print(cam.backgroundColor);
        Color c = Color.white - randomColor;
        c.a = 1;
        slogans[i].GetComponent<Text>().DOColor(c, 1f);

        /*for (int t=0; t<=10; t++)
        {
            GameObject shits = Instantiate(shit[Random.Range(0, shit.Length-1)], playArea.transform.position + new Vector3(30, 30, 30), Quaternion.identity) as GameObject;
            shits.GetComponent<Renderer>().material = gradients[Random.Range(0, gradients.Length-1)];

            Destroy(shits, 5f);
        }*/
    }

    void DisableSlogan()
    {
        slogans[index].SetActive(false);
    }

    void FindPlayer()
    {
        playerBody = GameObject.Find("[VRTK][AUTOGEN][BodyColliderContainer]");
        playArea = GameObject.Find("[VRTK_SDKManager]");
        print(anus);
        anus.transform.parent = playerBody.transform;
        asshole.transform.parent = playerBody.transform;
        butthole.transform.parent = playerBody.transform;
        anus.SetActive(true);
    }
}
