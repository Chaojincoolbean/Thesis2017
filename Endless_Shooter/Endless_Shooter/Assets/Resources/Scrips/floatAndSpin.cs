using UnityEngine;
using System.Collections;
using DG.Tweening;

public class floatAndSpin : MonoBehaviour
{
    GameObject leftController;
    GameObject rightController;

    //Called when this instance is initialized, once in a life time
    void Awake()
    {
        Rise();

    }

    // Called when this instance is enabled
    void Start()
    {

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        leftController = GameObject.Find("Controller (left)");
        rightController = GameObject.Find("Controller (right)");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity * gameObject.GetComponent<Rigidbody>().mass);
        }*/
        float distanceToLeftController = Vector3.Distance(leftController.transform.position, transform.position);
        float distanceToRightController = Vector3.Distance(rightController.transform.position, transform.position);
        if (distanceToLeftController<=0.5f||distanceToRightController<=0.5f)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity * gameObject.GetComponent<Rigidbody>().mass*1.1f);
            print("Controller is close");
            if (gameObject.GetComponent<Rigidbody>().isKinematic == true)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                print("This weapon is no longer kinematic");
            }

        }

    }

    void Rise()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        transform.DOMoveY(transform.position.y + 1f, 0.5f);
        //transform.DOLocalRotate(new Vector3(0, 359, 0), 1f,RotateMode.LocalAxisAdd).SetLoops(-1,LoopType.Restart);
    }


}
