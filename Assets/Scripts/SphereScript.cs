using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SphereScript : MonoBehaviour
{
    public Rigidbody rb;
    public XRGrabInteractable xrGrab;

    // Start is called before the first frame update
    void Awake()
    {
        xrGrab.onDeactivate.AddListener(ResetVelocity);
    }

    void ResetVelocity(XRBaseInteractor interactor)
    {
        rb.velocity = Vector3.zero;
    }

    
}
