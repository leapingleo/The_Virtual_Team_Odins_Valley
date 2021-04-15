using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 normal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, -transform.up, out hit, 0.5f);

        if (hit.transform != null)
        {
           // Debug.Log(hit.transform.name + " hittt");
            Debug.Log(hit.normal);
            normal = hit.normal;
            Debug.Log(normal.z);
            Physics.gravity = new Vector3(normal.x * -9.8f, normal.y * -9.8f, normal.z * -9.8f);
            
        }

        Vector3 lerpDir = Vector3.Lerp(transform.up, normal, 0.5f);
        transform.rotation = Quaternion.FromToRotation(transform.up, lerpDir) * transform.rotation;
        
    }
}
