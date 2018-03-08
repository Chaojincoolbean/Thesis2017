using UnityEngine;
using System.Collections;
using DG.Tweening;

public class floatAndSpin : MonoBehaviour
{
    bool isRisen = false;
    Vector3 RisenPos;
    // Use this for initialization
    void Start()
    {
        Rise();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity * gameObject.GetComponent<Rigidbody>().mass);
        }*/
        if (isRisen == true)
        {
            transform.position = RisenPos;
        }

    }

    void Rise()
    {
        transform.DOMoveY(transform.position.y + 1f, 0.5f).OnComplete(Freeze);
        //transform.DOLocalRotate(new Vector3(0, 359, 0), 1f,RotateMode.LocalAxisAdd).SetLoops(-1,LoopType.Restart);
    }

    void Freeze()
    {
        RisenPos = transform.position;
        isRisen = true;
    }

}
