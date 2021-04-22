using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectorGrabThrow : MonoBehaviour
{
    public float shootDistance;
    public GameObject indicatorSphere;
    private GameObject detectedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        int layer = LayerMask.GetMask("GrabThrow");

        if (Physics.Raycast(transform.position, transform.forward, out hit, 3, layer))
        {
            //show where the ray hit on object's surface
            SetIndicatorPos(hit.point);
            if (detectedObject == null && ActionController.Instance.TriggerButtonPressed)
            {
                detectedObject = hit.transform.gameObject;
            }
        } else {
            SetIndicatorPos(new Vector3(999, 999, 999));
        }

        if (ActionController.Instance.TriggerButtonPressed && detectedObject != null)
        {
            if (detectedObject.GetComponent<GrabThrow>().canBeGrabThrown)
                GrabObject(detectedObject);
            else
                detectedObject = null;
        }
        //if holding on to object and release, it will shoot it.
        else if (!ActionController.Instance.TriggerButtonPressed && detectedObject != null)
        {
            RaycastHit shootHit;
            if (Physics.Raycast(transform.position, transform.forward, out shootHit, shootDistance))
            {
                detectedObject.GetComponent<GrabThrow>().MoveToReleasePosition(shootHit.point);
            }
            else
            {
                detectedObject.GetComponent<GrabThrow>().MoveToReleasePosition(transform.position + transform.forward * shootDistance);

            }
            detectedObject = null;
        }
        else {
            detectedObject = null;
        }
    }

    void GrabObject(GameObject obj)
    {
        obj.GetComponent<GrabThrow>().MoveToHand(transform);
    }

    void SetIndicatorPos(Vector3 pos)
    {
        indicatorSphere.transform.position = pos;
    }
}