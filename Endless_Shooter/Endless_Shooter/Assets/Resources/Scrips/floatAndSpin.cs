using UnityEngine;
using System.Collections;
using DG.Tweening;

public class floatAndSpin : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        Invoke("Rise", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Rise()
    {
        transform.DOMoveY(transform.position.y + 1f, 0.5f);
        transform.DOLocalRotate(new Vector3(0, 359, 0), 1f,RotateMode.LocalAxisAdd).SetLoops(-1,LoopType.Restart);
    }

}
