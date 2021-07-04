using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionStay(Collision collision)
    {
        if ((collision.transform.CompareTag("Grabable") || collision.transform.CompareTag("Rotatable")) && transform.parent == null)
        {
            transform.parent = collision.transform;
        }

        if (collision.transform.CompareTag("Shuriken"))
        {
            transform.parent = collision.transform;
            collision.gameObject.GetComponent<Shuriken>().canBeGrabThrown = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        transform.parent = null;

        if (collision.transform.CompareTag("Shuriken"))
        {
            collision.gameObject.GetComponent<Shuriken>().canBeGrabThrown = true;
        }
    }
}
