using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropManager : MonoBehaviour {
    public GameObject[] drop;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void DropItem (Vector3 position)
    {
        GameObject droppedItem = Instantiate(drop[Random.Range(0, drop.Length)], position, Quaternion.identity) as GameObject;
        droppedItem.AddComponent<floatAndSpin>();
    }

}
