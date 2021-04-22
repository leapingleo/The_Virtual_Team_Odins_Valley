using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    public Vector3 force;
    Vector3 alignWithNormal;
    // Start is called before the first frame update
    void Start()
    {
        rb  = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (!hit.collider.CompareTag("Player")) {
                force = -hit.normal;
                alignWithNormal = hit.normal;
                
            }
                
        }
        transform.rotation = Quaternion.FromToRotation(transform.up, alignWithNormal) * transform.rotation;
        Quaternion q = Quaternion.LookRotation(alignWithNormal);

        Quaternion rotation = q;
        Vector3 myVector = new Vector3(0, 0, 1);
        Vector3 rotateVector = rotation * myVector;
      //  Debug.Log(rotateVector);
        rb.AddForce(force);
    }
}
