using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    public Vector3 direction;
    public float gravityForce;

    // Start is called before the first frame update
    void Start()
    {
        direction = direction.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyGravity()
    {
        Physics.gravity = direction * gravityForce;
    }

    public Vector3 RotateDirection()
    {
        return new Vector3(0.0f, 0.0f, -1.0f);
    }

    public Vector3 NewJumpDirection()
    {
        return new Vector3(0.0f, -1.0f, 0.0f);
    }
}
