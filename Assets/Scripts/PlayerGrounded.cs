using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    public bool grounded;
    //public LayerMask gravityLayers;
    //private Quaternion alignWithSurfaceRot;
    //private Vector3 groundNormal;

    //public Quaternion AlignWithSurfaceRot { get { return alignWithSurfaceRot; } }
    //public Vector3 GroundNormal { get { return groundNormal; } }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 10 && !other.CompareTag("GravityField"))
        {
            grounded = true;
        }

        

        //if (other.gameObject.CompareTag("GravityPlatform"))
        //{
        //    //RaycastHit hit;
        //    //Physics.Raycast(transform.position, -transform.up, out hit, 0.05f, gravityLayers);
        //    //if (hit.collider.CompareTag("GravityPlatform"))
        //    //    groundNormal = hit.normal;
        //    //else
        //    //    groundNormal = Vector3.up;

        //    Vector3 tmpDirection = (other.transform.position - transform.position);
        //    Vector3 tmpContactPoint = transform.position + tmpDirection;
        //    groundNormal = -tmpContactPoint;


        //    alignWithSurfaceRot = Quaternion.FromToRotation(Vector3.up, groundNormal);
        //}
        //else
        //{
        //    RaycastHit hit;
        //    Physics.Raycast(transform.position, -transform.up, out hit, 0.05f, gravityLayers);
        //    groundNormal = Vector3.up;
        //    alignWithSurfaceRot = Quaternion.FromToRotation(Vector3.up, Vector3.up);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 10 && !other.CompareTag("GravityField"))
            grounded = false;
    }


}
