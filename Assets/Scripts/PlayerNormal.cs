using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormal : MonoBehaviour
{
    public LayerMask gravityLayers;
    public LayerMask nonGravityLayers;
    private Quaternion alignWithSurfaceRot;
    private Vector3 groundNormal;

    public Quaternion AlignWithSurfaceRot { get { return alignWithSurfaceRot; } }
    public Vector3 GroundNormal { get { return groundNormal; } }

    private bool usingGravity = false;

    public bool UsingGravity { get { return usingGravity; } }

    public void FixedUpdate()
    {
        RaycastHit hit;

        
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.065f, gravityLayers))
        {
            groundNormal = hit.normal;
            usingGravity = true;
        }
        else if (Physics.Raycast(transform.position, -transform.up, out hit, 0.065f, nonGravityLayers))
        {
            groundNormal = Vector3.up;
            usingGravity = false;
        }


        alignWithSurfaceRot = Quaternion.FromToRotation(Vector3.up, groundNormal);
    }

    public void ResetGroundNormal()
    {
        groundNormal = Vector3.up;
    }
}
