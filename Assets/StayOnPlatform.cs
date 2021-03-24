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

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Grabable") || collision.transform.CompareTag("Rotatable"))
    //    {
    //        Vector3 platformAt = collision.transform.position;
    //        Vector3 offset = new Vector3(transform.position.x - platformAt.x, 0f, transform.position.z - platformAt.z);
    //        Vector3 followPos = new Vector3(platformAt.x + offset.x, 
    //               transform.position.y,
    //               platformAt.z + offset.z);
          
    //        transform.position = followPos;
    //    }
    //}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Grabable") || collision.transform.CompareTag("Rotatable"))
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        transform.parent = null;
    }
}
