using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityScale = 1.0f;
    public float globalGravity = -9.81f;

    Rigidbody rigidBody;

    public void SetRigidBody(Rigidbody rigidBody)
    {
        this.rigidBody = rigidBody;
    }

    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * transform.up;
        rigidBody.AddForce(gravity, ForceMode.Acceleration);
    }
}
